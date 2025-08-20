using DocumentManagement.API.Requests;
using Swashbuckle.AspNetCore.Filters;

namespace DocumentManagement.API.Swagger;

/// <summary>
/// Provides an example implementation of the <see cref="CreateNewDocumentRequest"/> class for Swagger UI.
/// </summary>
public class CreateNewDocumentRequestExample : IExamplesProvider<CreateNewDocumentRequest>
{
    /// <inheritdoc/>
    public CreateNewDocumentRequest GetExamples()
    {
        return new CreateNewDocumentRequest
        {
            Id = "some-unique-identifier1",
            Tags = ["important", ".net"],
            Created = DateTime.UtcNow,
            DataList =
            [
                new KeyValueEntry { Key = "some", Value = "data" },
                new KeyValueEntry { Key = "optional", Value = "fields" }
            ],
            Data = new Dictionary<string, string>()
            {
                ["some"] = "data",
                ["optional"] = "fields"
            }
        };
    }
}