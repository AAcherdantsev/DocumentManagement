using DocumentManagement.PublicModels.Documents;
using FluentResults;

namespace DocumentManagement.API.Services.Documents.Implementations;

public class DocumentService : IDocumentService
{
    public Task<Result> DeleteAsync(string id, CancellationToken ct = default)
    {
        throw new NotImplementedException();
    }

    public Task<Result<DocumentDto>> GetAsync(string id, CancellationToken ct = default)
    {
        throw new NotImplementedException();
    }

    public Task<Result> AddAsync(DocumentDto document, CancellationToken ct = default)
    {
        throw new NotImplementedException();
    }

    public Task<Result> UpdateAsync(string id, DocumentDto newDocument, CancellationToken ct = default)
    {
        throw new NotImplementedException();
    }
}