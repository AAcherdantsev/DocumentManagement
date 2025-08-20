using DocumentManagement.API.Requests;
using Swashbuckle.AspNetCore.Filters;

namespace DocumentManagement.API.Swagger;

/// <summary>
/// Provides an example implementation of the <see cref="UpdateDocumentRequest"/> class for use in API documentation and testing.
/// </summary>
public class UpdateDocumentRequestExample : IExamplesProvider<UpdateDocumentRequest>
{
    /// <inheritdoc/>
    public UpdateDocumentRequest GetExamples()
    {
        return new UpdateDocumentRequest
        {
            Id = "some-unique-id",
            NewTags = ["not-important", "java"],
            NewData = new Dictionary<string, string>
            {
                { "newKey1", "newValue1" },
                { "newKey2", "newValue2" }
            },
            NewDataList =
            [
                new KeyValueEntry { Key = "newKey1", Value = "newValue1" },
                new KeyValueEntry { Key = "newKey2", Value = "newValue2" }
            ]
        };
    }
}