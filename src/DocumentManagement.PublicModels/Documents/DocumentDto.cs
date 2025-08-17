namespace DocumentManagement.PublicModels.Documents;

/// <summary>
/// Represents a Data Transfer Object for a document, encapsulating its data.
/// </summary>
public record DocumentDto
{
    /// <summary>
    /// Gets the date and time when the document was created.
    /// </summary>
    public DateTime Created { get; init; } = DateTime.UtcNow;

    /// <summary>
    /// Gets the key-value pairs representing the associated data for the document.
    /// </summary>
    public required IDictionary<string, string> Data { get; init; }

    /// <summary>
    /// Gets the collection of tags associated with the document.
    /// </summary>
    public required IReadOnlySet<string> Tags { get; init; }

    /// <summary>
    /// Gets the unique identifier of the document.
    /// </summary>
    public required string Id { get; init; }
}