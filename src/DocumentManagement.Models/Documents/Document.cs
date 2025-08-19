using System.Collections.Frozen;

namespace DocumentManagement.Models.Documents;

/// <summary>
/// Represents a document containing data, tags, and data content.
/// </summary>
public class Document
{
    /// <summary>
    /// Gets the date and time when the document was created.
    /// </summary>
    public DateTime Created { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Gets or sets the date and time when the document was last updated.
    /// </summary>
    public DateTime? LastUpdated { get; set; }

    /// <summary>
    /// Gets the key-value pairs representing the associated data for the document.
    /// </summary>
    public required Dictionary<string, string> Data { get; set; }

    /// <summary>
    /// Gets the collection of tags associated with the document.
    /// </summary>
    public required HashSet<string> Tags { get; set; }

    /// <summary>
    /// Gets the unique identifier of the document.
    /// </summary>
    public required string Id { get; init; }
}