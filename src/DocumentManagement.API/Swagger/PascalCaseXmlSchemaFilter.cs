using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace DocumentManagement.API.Swagger;

/// <summary>
/// A custom implementation of ISchemaFilter that modifies the OpenAPI schema by converting property keys
/// to PascalCase.
/// </summary>
public class PascalCaseXmlSchemaFilter : ISchemaFilter
{
    public void Apply(OpenApiSchema schema, SchemaFilterContext context)
    {
        if (schema.Properties == null)
            return;

        var props = schema.Properties.Keys.ToList();
        foreach (var key in props)
        {
            var value = schema.Properties[key];
            schema.Properties.Remove(key);
            var newKey = char.ToUpper(key[0]) + key.Substring(1);
            schema.Properties[newKey] = value;
        }
    }
}