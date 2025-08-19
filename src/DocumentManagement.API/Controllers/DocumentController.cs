using DocumentManagement.API.Requests;
using DocumentManagement.API.Services.Documents;
using DocumentManagement.PublicModels.Documents;
using DocumentManagement.PublicModels.Errors;
using Microsoft.AspNetCore.Mvc;
using FluentResults;

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
    
    /// <summary>
    /// Creates a new instance of the <see cref="DocumentController"/> class.
    /// </summary>
    /// <param name="documentService">The document service.</param>
    /// <param name="logger">The logger.</param>
    public DocumentController(IDocumentService documentService, ILogger<DocumentController> logger)
    {
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
    public async Task<ActionResult<DocumentDto>> GetDocument(string id)
    {
        _logger.LogInformation("Getting document with id {id}...", id);
        
        var result = await _documentService.GetAsync(id);
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
    public async Task<ActionResult> AddDocument([FromBody] CreateNewDocumentRequest request)
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
            _logger.LogInformation("Document with id {id} added successfully.", request.Id);
            return CreatedAtAction(nameof(GetDocument), new { id = request.Id }, null);
        }
        
        return HandleError(result, request.Id);
    }

    /// <summary>
    /// Updates an existing document with the provided data.
    /// </summary>
    /// <param name="id">The unique identifier of the document to be updated.</param>
    /// <param name="document">The updated document data.</param>
    /// <returns>An <see cref="ActionResult"/> indicating the outcome of the operation.</returns>
    [HttpPut("{id}")]
    public async Task<ActionResult> UpdateDocument([FromRoute] string id, [FromBody] DocumentDto document)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        
        _logger.LogInformation("Updating document with id {id}...", document.Id);
        
        var result = await _documentService.UpdateAsync(id, document);
        
        if (result.IsSuccess)
        {
            _logger.LogInformation("Document with id {id} updated successfully.", document.Id);
            return Ok();
        }
        
        return HandleError(result, id);
    }

    /// <summary>
    /// Deletes a document by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the document to delete.</param>
    /// <returns>An <see cref="ActionResult"/> indicating the result of the deletion operation.</returns>
    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteDocument(string id)
    {
        _logger.LogInformation("Deleting document with id {id}...", id);
        
        var result = await _documentService.DeleteAsync(id);

        if (result.IsSuccess)
        {
            _logger.LogInformation("Document with id {id} deleted successfully.", id);
            return NoContent();
        }
        
        return HandleError(result, id);
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

    /// <summary>
    /// Updates an existing document with the given data.
    /// </summary>
    /// <param name="request">The request.</param>
    /// <returns>Returns an HTTP response indicating the result of the update operation.</returns>
    [HttpPatch]
    public async Task<ActionResult> PatchDocument([FromBody] UpdateDocumentRequest request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        
        _logger.LogInformation("Updating document with id {id}...", request.Id);
        
        var result = await _documentService.UpdateAsync(request);
        
        if (result.IsSuccess)
        {
            _logger.LogInformation("Document with id {id} updated successfully.", request.Id);
            return Ok();
        }
        
        return HandleError(result, request.Id);
    }
}