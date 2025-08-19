using AutoMapper;
using DocumentManagement.API.Requests;
using DocumentManagement.Models.Documents;
using DocumentManagement.Persistence.DocumentRepositories;
using DocumentManagement.PublicModels.Documents;
using FluentResults;

namespace DocumentManagement.API.Services.Documents.Implementations;

/// <summary>
/// Provides services for managing documents within the application.
/// </summary>
public class DocumentService : IDocumentService
{
    private readonly IDocumentRepository _documentRepository;
    private readonly IMapper _mapper;
    
    public DocumentService(IDocumentRepository documentRepository, IMapper mapper)
    {
        _documentRepository = documentRepository;
        _mapper = mapper;
    }
    
    /// <inheritdoc/>
    public Task<Result> DeleteAsync(string id, CancellationToken ct = default)
    {
        return _documentRepository.DeleteAsync(id, ct);
    }

    /// <inheritdoc/>
    public async Task<Result<DocumentDto>> GetAsync(string id, CancellationToken ct = default)
    {
        var result = await _documentRepository.GetAsync(id, ct);

        if (result.IsSuccess)
        {
            return Result.Ok(_mapper.Map<DocumentDto>(result.Value));
        }

        return Result.Fail(result.Errors);
    }

    /// <inheritdoc/>
    public async Task<Result> AddAsync(DocumentDto document, CancellationToken ct = default)
    {
        var documentModel = _mapper.Map<Document>(document);
        
        documentModel.Created = DateTime.UtcNow;
        documentModel.LastUpdated = DateTime.UtcNow;
        
        var result = await _documentRepository.AddAsync(documentModel, ct);
        return result;
    }

    /// <inheritdoc/>
    public async Task<Result> UpdateAsync(string id, DocumentDto newDocument, CancellationToken ct = default)
    {
        var documentModel = _mapper.Map<Document>(newDocument);
        return await _documentRepository.UpdateAsync(id, documentModel, ct);
    }

    /// <inheritdoc/>
    public async Task<Result> UpdateAsync(UpdateDocumentRequest request, CancellationToken ct = default)
    {
        var result = await _documentRepository.GetAsync(request.Id, ct);

        if (result.IsSuccess)
        {
            var document = result.Value;
            document.LastUpdated = DateTime.UtcNow;
            
            if (request.NewData != null)
            {
                document.Data = request.NewData;
            }
            
            if (request.NewTags != null)
            {
                document.Tags = request.NewTags;
            }
            
            return await _documentRepository.UpdateAsync(document.Id, document, ct);
        }
        
        return Result.Fail(result.Errors);
    }
}