using DocumentManagement.API.Services.Documents;
using DocumentManagement.API.Services.Documents.Implementations;
using DocumentManagement.Persistence.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddControllers();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<IDocumentService, DocumentService>();
builder.Services.AddInMemoryDocumentRepository();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();
app.UseHttpsRedirection();
app.Run();