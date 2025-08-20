using System.Text.Json.Serialization;
using System.Xml.Serialization;
using MessagePack;

namespace DocumentManagement.API.Requests;

/// <summary>
/// Represents a request to create a new document.
/// </summary>
[MessagePackObject]
[XmlRoot("CreateNewDocumentRequest")]
public class CreateNewDocumentRequest
{
    /// <summary>
    /// Gets or initializes the unique identifier for the document.
    /// </summary>
    [Key(0)]
    [XmlElement("Id")]
    public required string Id { get; init; }

    /// <summary>
    /// Gets or initializes the date and time when the document was created.
    /// </summary>
    [Key(1)]
    [XmlElement("Created")]
    public required DateTime Created { get; init; }

    /// <summary>
    /// Gets or initializes the list of tags associated with the document.
    /// </summary>
    [Key(2)]
    [XmlArray("Tags")]
    [XmlArrayItem("Tag")]
    public required List<string> Tags { get; init; }

    /// <summary>
    /// Gets or sets the collection of key-value pairs representing additional data associated with the document.
    /// </summary>
    [Key(3)]
    [XmlIgnore]
    public Dictionary<string, string> Data { get; set; } = [];

    /// <summary>
    /// Represents a collection of key-value entries to store additional data for the document.
    /// </summary>
    [XmlArray("Data")]
    [XmlArrayItem("Entry")]
    [IgnoreMember]
    [JsonIgnore]
    public List<KeyValueEntry> DataList { get; set; } = [];
}
