using DocumentManagement.Models.Documents;
using FluentResults;

namespace DocumentManagement.Persistence.DocumentRepositories;

/// <summary>
/// Defines a contract for repository operations related to document management.
/// </summary>
public interface IDocumentRepository
{
    /// <summary>
    /// Asynchronously retrieves a document based on the provided identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the document to retrieve.</param>
    /// <param name="ct">The cancellation token to observe while waiting for the task to complete.</param>
    /// <returns>A task containing the result of the operation, including the retrieved <see cref="Document"/> if successful.</returns>
    Task<Result<Document>> GetAsync(string id, CancellationToken ct = default);

    /// <summary>
    /// Asynchronously adds a new document to the repository.
    /// </summary>
    /// <param name="document">The <see cref="Document"/> representing the document to be added.</param>
    /// <param name="ct">The cancellation token to observe while waiting for the task to complete.</param>
    /// <returns>A task containing the result of the operation.</returns>
    Task<Result> AddAsync(Document document, CancellationToken ct = default);

    /// <summary>
    /// Asynchronously updates an existing document in the repository with the provided identifier and new data.
    /// </summary>
    /// <param name="id">The unique identifier of the document to update.</param>
    /// <param name="document">The updated <see cref="Document"/> containing the new data and tags.</param>
    /// <param name="ct">The cancellation token to observe while waiting for the task to complete.</param>
    /// <returns>A task containing the result of the operation.</returns>
    Task<Result> UpdateAsync(string id, Document document, CancellationToken ct = default);

    /// <summary>
    /// Asynchronously deletes a document based on the provided identifier from the repository.
    /// </summary>
    /// <param name="id">The unique identifier of the document to delete.</param>
    /// <param name="ct">The cancellation token to observe while waiting for the task to complete.</param>
    /// <returns>A task containing the result of the operation.</returns>
    Task<Result> DeleteAsync(string id, CancellationToken ct = default);
}