using DocumentManagement.API.Controllers;
using DocumentManagement.API.Requests;
using DocumentManagement.API.Services.Documents;
using DocumentManagement.PublicModels.Documents;
using DocumentManagement.PublicModels.Errors;
using FluentResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;

namespace DocumentManagement.API.Tests.Controllers;

[TestFixture]
public class DocumentControllerTests
{
    private Mock<IDocumentService> _serviceMock;
    private Mock<ILogger<DocumentController>> _loggerMock;
    private DocumentController _controller;

    [SetUp]
    public void Setup()
    {
        _serviceMock = new Mock<IDocumentService>();
        _loggerMock = new Mock<ILogger<DocumentController>>();
        _controller = new DocumentController(_serviceMock.Object, _loggerMock.Object);
    }

    [Test]
    public async Task GetDocument_ShouldReturnOk_WhenFound()
    {
        var dto = new DocumentDto
        {
            Id = "d1", 
            Data = new Dictionary<string, string>()
            {
                ["key"] = "value"
            },
            Tags = ["tag"]
        };
        _serviceMock.Setup(s => s.GetAsync("d1", It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Ok(dto));

        var result = await _controller.GetDocument("d1");

        Assert.That(result.Result, Is.TypeOf<OkObjectResult>());
        var ok = result.Result as OkObjectResult;
        Assert.That(ok.Value, Is.EqualTo(dto));
    }

    [Test]
    public async Task GetDocument_ShouldReturnNotFound_WhenMissing()
    {
        _serviceMock.Setup(s => s.GetAsync("ghost", It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Fail<DocumentDto>(new NotFoundError("not found")));

        var result = await _controller.GetDocument("ghost");

        Assert.That(result.Result, Is.TypeOf<NotFoundResult>());
    }

    [Test]
    public async Task AddDocument_ShouldReturnCreated_WhenSuccess()
    {
        var request = new CreateNewDocumentRequest
        {
            Id = "new1", 
            Tags = [ "t" ], 
            Data = []
        };
        _serviceMock.Setup(s => s.AddAsync(It.IsAny<DocumentDto>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Ok());

        var result = await _controller.AddDocument(request);

        Assert.That(result, Is.TypeOf<CreatedAtActionResult>());
        var created = result as CreatedAtActionResult;
        Assert.That(created.ActionName, Is.EqualTo(nameof(DocumentController.GetDocument)));
    }

    [Test]
    public async Task AddDocument_ShouldReturnConflict_WhenDuplicate()
    {
        var request = new CreateNewDocumentRequest
        {
            Id = "dup", 
            Tags = [], 
            Data = []
        };
        _serviceMock.Setup(s => s.AddAsync(It.IsAny<DocumentDto>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Fail(new ConflictError("exists")));

        var result = await _controller.AddDocument(request);

        Assert.That(result, Is.TypeOf<ConflictResult>());
    }

    [Test]
    public async Task UpdateDocument_ShouldReturnOk_WhenSuccess()
    {
        var dto = new DocumentDto
        {
            Id = "d2", 
            Tags = [], 
            Data = [],
        };
        _serviceMock.Setup(s => s.UpdateAsync("d2", dto, It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Ok());

        var result = await _controller.UpdateDocument("d2", dto);

        Assert.That(result, Is.TypeOf<OkResult>());
    }

    [Test]
    public async Task UpdateDocument_ShouldReturnNotFound_WhenMissing()
    {
        var dto = new DocumentDto
        {
            Id = "nope",
            Tags = [], 
            Data = [],
        };
        _serviceMock.Setup(s => s.UpdateAsync("nope", dto, It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Fail(new NotFoundError("nope")));

        var result = await _controller.UpdateDocument("nope", dto);

        Assert.That(result, Is.TypeOf<NotFoundResult>());
    }

    [Test]
    public async Task DeleteDocument_ShouldReturnNoContent_WhenSuccess()
    {
        _serviceMock.Setup(s => s.DeleteAsync("d3", It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Ok());

        var result = await _controller.DeleteDocument("d3");

        Assert.That(result, Is.TypeOf<NoContentResult>());
    }

    [Test]
    public async Task DeleteDocument_ShouldReturnNotFound_WhenMissing()
    {
        _serviceMock.Setup(s => s.DeleteAsync("ghost", It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Fail(new NotFoundError("not found")));

        var result = await _controller.DeleteDocument("ghost");

        Assert.That(result, Is.TypeOf<NotFoundResult>());
    }

    [Test]
    public async Task PatchDocument_ShouldReturnOk_WhenSuccess()
    {
        var request = new UpdateDocumentRequest
        {
            Id = "patch", 
            NewTags = [ "x" ]
        };
        _serviceMock.Setup(s => s.UpdateAsync(request, It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Ok());

        var result = await _controller.PatchDocument(request);

        Assert.That(result, Is.TypeOf<OkResult>());
    }

    [Test]
    public async Task PatchDocument_ShouldReturnNotFound_WhenMissing()
    {
        var request = new UpdateDocumentRequest { Id = "ghost" };
        _serviceMock.Setup(s => s.UpdateAsync(request, It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Fail(new NotFoundError("ghost")));

        var result = await _controller.PatchDocument(request);

        Assert.That(result, Is.TypeOf<NotFoundResult>());
    }
}