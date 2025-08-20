using System.Reflection;
using System.Text.Json;
using DocumentManagement.API.Mapping;
using DocumentManagement.API.Services.Documents;
using DocumentManagement.API.Services.Documents.Implementations;
using DocumentManagement.API.Swagger;
using DocumentManagement.Persistence.Extensions;
using Swashbuckle.AspNetCore.Filters;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services
    .AddControllers(options =>
    {
        options.RespectBrowserAcceptHeader = true;
    })
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
    })
    .AddMessagePackFormatters()
    .AddXmlSerializerFormatters();

builder.Services.AddAutoMapper(typeof(MappingProfile));
builder.Services.AddSwaggerGen(c =>
{
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    c.IncludeXmlComments(xmlPath, includeControllerXmlComments: true);
    c.SchemaFilter<PascalCaseXmlSchemaFilter>();
    c.ExampleFilters();
});
builder.Services.AddInMemoryDocumentRepository();
builder.Services.AddMemoryCache();
builder.Services.AddSingleton<IDocumentService, DocumentService>();
builder.Services.AddSwaggerExamplesFromAssemblyOf<Program>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();
app.UseHttpsRedirection();
app.Run();