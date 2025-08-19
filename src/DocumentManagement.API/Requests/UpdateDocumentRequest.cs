namespace DocumentManagement.API.Requests;

/// <summary>
/// Represents a request to update a document's data or tags in the system.
/// </summary>
public class UpdateDocumentRequest
{
    /// <summary>
    /// Gets the unique identifier of the document.
    /// </summary>
    public required string Id { get; init; }
    
    /// <summary>
    /// Gets the key-value pairs representing the associated data for the document.
    /// </summary>
    public Dictionary<string, string>? NewData { get; init; } 

    /// <summary>
    /// Gets the collection of tags associated with the document.
    /// </summary>
    public HashSet<string>? NewTags { get; init; }
}