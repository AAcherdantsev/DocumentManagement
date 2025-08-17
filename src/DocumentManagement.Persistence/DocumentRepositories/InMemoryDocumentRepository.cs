using DocumentManagement.API.Services.Documents;
using DocumentManagement.Models.Documents;
using FluentResults;

namespace DocumentManagement.Persistence.DocumentRepositories;

/// <summary>
/// Provides an in-memory implementation of the <see cref="IDocumentRepository"/> interface.
/// </summary>
internal class InMemoryDocumentRepository : IDocumentRepository
{
    private readonly Dictionary<string, Document> _documents = new();
    
    /// <inheritdoc/>
    public Task<Result<Document>> GetAsync(string id, CancellationToken ct = default)
    {
        if (_documents.TryGetValue(id, out Document? document))
        {
            return Task.FromResult(Result.Ok(document));
        }
        
        return Task.FromResult(Result.Fail<Document>("Document not found"));
    }

    /// <inheritdoc/>
    public Task<Result> AddAsync(Document document, CancellationToken ct = default)
    {
        if (_documents.ContainsKey(document.Id))
        {
            return Task.FromResult(Result.Fail($"Document with id {document.Id} already exists"));
        }
        _documents.Add(document.Id, document);

        return Task.FromResult(Result.Ok());
    }

    /// <inheritdoc/>
    public Task<Result> UpdateAsync(string id, Document document, CancellationToken ct = default)
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc/>
    public Task<Result> DeleteAsync(string id, CancellationToken ct = default)
    {
        if (_documents.Remove(id, out _))
        {
            return Task.FromResult(Result.Ok());
        }

        return Task.FromResult(Result.Fail("Document not found"));
    }
}