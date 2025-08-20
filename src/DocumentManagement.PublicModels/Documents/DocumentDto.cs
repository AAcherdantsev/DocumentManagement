using System.Text.Json.Serialization;
using System.Xml.Serialization;
using MessagePack;

namespace DocumentManagement.PublicModels.Documents;

/// <summary>
/// Represents a Data Transfer Object for a document, encapsulating its data.
/// </summary>
[MessagePackObject]
[XmlRoot("DocumentDto")]
public record DocumentDto
{
    /// <summary>
    /// Gets the date and time when the document was created.
    /// </summary>
    [Key(0)]
    public DateTime Created { get; init; } = DateTime.UtcNow;

    /// <summary>
    /// Gets the date and time when the document was last updated, if available.
    /// </summary>
    [Key(1)]
    public DateTime? LastUpdated { get; init; }

    /// <summary>
    /// Gets the key-value pairs representing the associated data for the document.
    /// </summary>
    [Key(2)]
    [XmlIgnore]
    public required Dictionary<string, string> Data { get; init; } 

    /// <summary>
    /// Gets the collection of tags associated with the document.
    /// </summary>
    [Key(3)]
    [XmlArray("Tags")]
    [XmlArrayItem("Tag")]
    public required List<string> Tags { get; init; }

    /// <summary>
    /// Gets the unique identifier of the document.
    /// </summary>
    [Key(4)]
    public required string Id { get; init; }
    
    [XmlArray("Data")]
    [XmlArrayItem("Entry")]
    [IgnoreMember]
    [JsonIgnore]
    public List<KeyValueEntry> DataList { get; set; } = [];
}

/// <summary>
/// Represents a key-value pair entry
/// </summary>
public class KeyValueEntry
{
    [XmlElement("Key")]
    public string Key { get; set; } = null!;

    [XmlElement("Value")]
    public string Value { get; set; } = null!;
}