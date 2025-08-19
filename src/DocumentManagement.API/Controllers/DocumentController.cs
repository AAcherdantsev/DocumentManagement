using DocumentManagement.API.Requests;
using DocumentManagement.API.Services.Documents;
using DocumentManagement.PublicModels.Documents;
using DocumentManagement.PublicModels.Errors;
using Microsoft.AspNetCore.Mvc;
using FluentResults;
using Microsoft.Extensions.Caching.Memory;

namespace DocumentManagement.API.Controllers;

/// <summary>
/// Handles operations related to document management, such as fetching, creating, updating, and deleting documents.
/// </summary>
[ApiController]
[Route("documents")]
[Produces("application/json", "application/xml", "application/x-msgpack")]
[Consumes("application/json", "application/xml", "application/x-msgpack")]
public class DocumentController : ControllerBase
{
    private readonly IDocumentService _documentService;
    private readonly ILogger<DocumentController> _logger;
    private readonly IMemoryCache _cache;
    
    /// <summary>
    /// Creates a new instance of the <see cref="DocumentController"/> class.
    /// </summary>
    /// <param name="documentService">The document service.</param>
    /// <param name="cache">The cache.</param>
    /// <param name="logger">The logger.</param>
    public DocumentController(IDocumentService documentService, IMemoryCache cache, ILogger<DocumentController> logger)
    {
        _cache = cache;
        _logger = logger;
        _documentService = documentService;
    }

    /// <summary>
    /// Retrieves a document by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the document to retrieve.</param>
    /// <returns>Returns an <see cref="ActionResult{T}"/> containing a <see cref="DocumentDto"/>
    /// if the operation is successful; otherwise, an appropriate error response.</returns>
    [HttpGet("{id}")]
    public async Task<ActionResult<DocumentDto>> GetDocumentAsync(string id)
    {
        if (_cache.TryGetValue<DocumentDto>(id, out var cachedDoc))
        {
            if (cachedDoc == null)
                return NotFound();
            
            _logger.LogInformation("Returning cached document {id}", id);
            return Ok(cachedDoc);
        }
        _logger.LogInformation("Getting document with id {id}...", id);
        
        var result = await _documentService.GetAsync(id);
        
        _cache.Set(id, result.IsSuccess ? result.Value : null);
        
        if (result.IsSuccess)
        {
            return Ok(result.Value);
        }
        return HandleError(result, id);
    }

    /// <summary>
    /// Adds a new document to the document management system.
    /// </summary>
    /// <param name="request">The <see cref="CreateNewDocumentRequest"/> representing the document to be added.</param>
    /// <returns>A <see cref="Task{ActionResult}"/> representing the result of the operation,
    /// such as a success message or an error response.</returns>
    [HttpPost]
    public async Task<ActionResult> AddDocumentAsync([FromBody] CreateNewDocumentRequest request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        
        _logger.LogInformation("Adding document with id {id}...", request.Id);

        var document = new DocumentDto()
        {
            Id = request.Id,
            Data = request.Data,
            Tags = request.Tags,
            Created = DateTime.UtcNow,
            LastUpdated = DateTime.UtcNow,
        };

        var result = await _documentService.AddAsync(document);
        
        if (result.IsSuccess)
        {
            if (_cache.TryGetValue<DocumentDto>(document.Id, out _))
            {
                _cache.Set(document.Id, document);
            }
            
            _logger.LogInformation("Document with id {id} added successfully.", request.Id);
            
            return CreatedAtAction(
                "GetDocument", 
                new { id = request.Id }, 
                document
            );
        }
        
        return HandleError(result, request.Id);
    }

    /// <summary>
    /// Deletes a document by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the document to delete.</param>
    /// <returns>An <see cref="ActionResult"/> indicating the result of the deletion operation.</returns>
    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteDocumentAsync(string id)
    {
        _logger.LogInformation("Deleting document with id {id}...", id);
        
        var result = await _documentService.DeleteAsync(id);

        if (result.IsSuccess)
        {
            if (_cache.TryGetValue<DocumentDto>(id, out _))
            {
                _cache.Set<DocumentDto>(id, null!);
            }
            
            _logger.LogInformation("Document with id {id} deleted successfully.", id);
            return NoContent();
        }
        
        return HandleError(result, id);
    }
    
    /// <summary>
    /// Updates an existing document with the given data.
    /// </summary>
    /// <param name="request">The request.</param>
    /// <returns>Returns an HTTP response indicating the result of the update operation.</returns>
    [HttpPatch]
    public async Task<ActionResult> PatchDocumentAsync([FromBody] UpdateDocumentRequest request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        
        _logger.LogInformation("Updating document with id {id}...", request.Id);
        
        var result = await _documentService.UpdateAsync(request);
        
        if (result.IsSuccess)
        {
            _cache.Remove(request.Id);
            _logger.LogInformation("Document with id {id} updated successfully.", request.Id);
            return Ok();
        }
        
        return HandleError(result, request.Id);
    }
    
    /// <summary>
    /// Handles errors encountered during document operations and returns the appropriate HTTP response.
    /// </summary>
    /// <param name="result">The result object containing the error information.</param>
    /// <param name="id">The identifier of the document related to the error.</param>
    /// <returns>An <see cref="ActionResult"/> representing the appropriate HTTP response based on the error.</returns>
    private ActionResult HandleError(ResultBase result, string id)
    {
        if (result.HasError<NotFoundError>())
        {
            _logger.LogWarning("Document {id} not found", id);
            return NotFound();
        }
        
        if (result.HasError<TimeoutError>())
        {
            _logger.LogError("Timeout operation with document {id}", id);
            return StatusCode(StatusCodes.Status504GatewayTimeout);
        }
        
        if (result.HasError<ConflictError>())
        {
            _logger.LogWarning("Document {id} already exists", id);
            return Conflict();
        }
        
        _logger.LogError("The operation with document failed. Document {id}, Errors: {Errors}", 
            id, string.Join(", ", result.Errors));
        
        return StatusCode(StatusCodes.Status500InternalServerError);
    }
}