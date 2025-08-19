namespace DocumentManagement.API.Requests;

/// <summary>
/// Represents a request to create a new document in the document management system.
/// </summary>
public class CreateNewDocumentRequest
{
    /// <summary>
    /// Gets the key-value pairs representing the associated data for the document.
    /// </summary>
    public required Dictionary<string, string> Data { get; init; } 

    /// <summary>
    /// Gets the collection of tags associated with the document.
    /// </summary>
    public required HashSet<string> Tags { get; init; }

    /// <summary>
    /// Gets the unique identifier of the document.
    /// </summary>
    public required string Id { get; init; }
}