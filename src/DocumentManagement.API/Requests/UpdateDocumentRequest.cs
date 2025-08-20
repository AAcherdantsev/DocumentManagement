using System.Text.Json.Serialization;
using System.Xml.Serialization;
using MessagePack;

namespace DocumentManagement.API.Requests;

/// <summary>
/// Represents a request to update a document's data or tags in the system.
/// </summary>
[MessagePackObject]
[XmlRoot("UpdateDocumentRequest")]
public class UpdateDocumentRequest
{
    /// <summary>
    /// Gets the unique identifier of the document.
    /// </summary>
    [Key(0)]
    public required string Id { get; init; }

    /// <summary>
    /// Gets the list of tags associated with the document to be updated.
    /// </summary>
    [Key(1)]
    [XmlArray("NewTags")]
    [XmlArrayItem("Tag")]
    public List<string>? NewTags { get; init; }
    
    /// <summary>
    /// Gets or sets a dictionary containing the updated key-value pairs for the document's data.
    /// </summary>
    [Key(2)]
    [XmlIgnore]
    public Dictionary<string, string>? NewData { get; set; } = [];
    
    /// <summary>
    /// Gets or sets the list representation of new data entries,
    /// with each entry represented as a key-value pair.
    /// </summary>
    [Key(3)]
    [JsonIgnore]
    [IgnoreMember]
    [XmlArray("NewData")]
    [XmlArrayItem("Entry")]
    public List<KeyValueEntry>? NewDataList { get; set; } = [];
}