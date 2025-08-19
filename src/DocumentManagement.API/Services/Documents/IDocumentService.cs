using DocumentManagement.API.Requests;
using DocumentManagement.PublicModels.Documents;
using FluentResults;

namespace DocumentManagement.API.Services.Documents;

/// <summary>
/// Provides an abstraction for managing documents.
/// </summary>
public interface IDocumentService
{
    /// <summary>
    /// Deletes a document based on its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the document to delete.</param>
    /// <param name="ct">A <see cref="CancellationToken"/> used to observe cancellation requests.</param>
    /// <returns>A <see cref="Result"/> indicating success or failure of the operation.</returns>
    Task<Result> DeleteAsync(string id, CancellationToken ct = default);

    /// <summary>
    /// Retrieves a document based on its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the document to retrieve.</param>
    /// <param name="ct">A <see cref="CancellationToken"/> used to observe cancellation requests.</param>
    /// <returns>A <see cref="Result{T}"/> containing the <see cref="DocumentDto"/> if the retrieval is successful, or an error result otherwise.</returns>
    Task<Result<DocumentDto>> GetAsync(string id, CancellationToken ct = default);

    /// <summary>
    /// Adds a new document to the system.
    /// </summary>
    /// <param name="document">The <see cref="DocumentDto"/> representing the document to be added.</param>
    /// <param name="ct">A <see cref="CancellationToken"/> used to observe cancellation requests.</param>
    /// <returns>A <see cref="Result"/> indicating success or failure of the operation.</returns>
    Task<Result> AddAsync(DocumentDto document, CancellationToken ct = default);

    /// <summary>
    /// Updates an existing document identified by its unique identifier with new data.
    /// </summary>
    /// <param name="id">The unique identifier of the document to update.</param>
    /// <param name="newDocument">The updated document data to replace the existing one.</param>
    /// <param name="ct">A <see cref="CancellationToken"/> used to observe cancellation requests.</param>
    /// <returns>A <see cref="Result"/> indicating success or failure of the update operation.</returns>
    Task<Result> UpdateAsync(string id, DocumentDto newDocument, CancellationToken ct = default);
    
    
    Task<Result> UpdateAsync(UpdateDocumentRequest request, CancellationToken ct = default);
}