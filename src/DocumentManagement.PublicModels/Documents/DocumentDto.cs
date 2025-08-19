using System.Collections.Frozen;

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
    /// Gets the date and time when the document was last updated, if available.
    /// </summary>
    public DateTime? LastUpdated { get; init; }

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