using System.Collections.Concurrent;
using System.Runtime.CompilerServices;
using DocumentManagement.Models.Documents;
using DocumentManagement.PublicModels.Errors;
using FluentResults;

namespace DocumentManagement.Persistence.DocumentRepositories;

/// <summary>
/// Provides an in-memory implementation of the <see cref="IDocumentRepository"/> interface.
/// </summary>
internal class InMemoryDocumentRepository : IDocumentRepository
{
    private readonly ConcurrentDictionary<string, Document> _documents = new();
    
    /// <inheritdoc/>
    public Task<Result<Document>> GetAsync(string id, CancellationToken ct = default)
    {
        if (_documents.TryGetValue(id, out Document? document))
        {
            return Task.FromResult(Result.Ok(document));
        }
        return Task.FromResult(Result.Fail<Document>(new NotFoundError("Document not found")));
    }

    /// <inheritdoc/>
    public Task<Result> AddAsync(Document document, CancellationToken ct = default)
    {
        if (_documents.ContainsKey(document.Id) || !_documents.TryAdd(document.Id, document))
        {
            return Task.FromResult(Result.Fail(new ConflictError("Document already exists")));
        }
        
        return Task.FromResult(Result.Ok());
    }

    /// <inheritdoc/>
    public Task<Result> UpdateAsync(string id, Document document, CancellationToken ct = default)
    {
        if (_documents.TryGetValue(id, out _))
        {
            _documents[id] = document;
            return Task.FromResult(Result.Ok());
        }
        return Task.FromResult(Result.Fail(new NotFoundError("Document not found")));
    }

    /// <inheritdoc/>
    public Task<Result> DeleteAsync(string id, CancellationToken ct = default)
    {
        if (_documents.Remove(id, out _))
        {
            return Task.FromResult(Result.Ok());
        }
        return Task.FromResult(Result.Fail(new NotFoundError("Document not found")));
    }
}