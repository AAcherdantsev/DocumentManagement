using System.Text.Json;
using DocumentManagement.API.Mapping;
using DocumentManagement.API.Services.Documents;
using DocumentManagement.API.Services.Documents.Implementations;
using DocumentManagement.Persistence.Extensions;
using MessagePack.AspNetCoreMvcFormatter;
using Microsoft.AspNetCore.Mvc.Formatters;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services
    .AddControllers(options => 
    {
        options.RespectBrowserAcceptHeader = true;
        options.InputFormatters.Add(new MessagePackInputFormatter());
        options.InputFormatters.Add(new XmlSerializerInputFormatter(options));
        options.OutputFormatters.Add(new MessagePackOutputFormatter());
        options.OutputFormatters.Add(new XmlSerializerOutputFormatter());
    })
    .AddJsonOptions(options => 
    {
        options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
    })
    .AddXmlSerializerFormatters();

builder.Services.AddAutoMapper(typeof(MappingProfile));
builder.Services.AddSwaggerGen();
builder.Services.AddInMemoryDocumentRepository();
builder.Services.AddMemoryCache();
builder.Services.AddSingleton<IDocumentService, DocumentService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();
app.UseHttpsRedirection();
app.Run();