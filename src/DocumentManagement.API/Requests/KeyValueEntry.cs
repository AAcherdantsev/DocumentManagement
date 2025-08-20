using System.Xml.Serialization;
using MessagePack;

namespace DocumentManagement.API.Requests;

/// <summary>
/// Represents a key-value entry used for data storage or transfer in a collection.
/// </summary>
[MessagePackObject]
public class KeyValueEntry
{
    /// <summary>
    /// Represents the unique key associated with the entry.
    /// </summary>
    [XmlElement("Key")]
    [Key(0)]
    public string Key { get; set; } = null!;

    /// <summary>
    /// Gets or sets the associated value for the key in the key-value entry.
    /// </summary>
    [XmlElement("Value")]
    [Key(1)]
    public string Value { get; set; } = null!;
}