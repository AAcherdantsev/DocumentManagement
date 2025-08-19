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
        // Arrange
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

        // Act
        var result = await _controller.GetDocumentAsync("d1");

        // Assert
        Assert.That(result.Result, Is.TypeOf<OkObjectResult>());
        var ok = result.Result as OkObjectResult;
        Assert.That(ok.Value, Is.EqualTo(dto));
    }

    [Test]
    public async Task GetDocument_ShouldReturnNotFound_WhenMissing()
    {
        // Arrange
        _serviceMock.Setup(s => s.GetAsync("ghost", It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Fail<DocumentDto>(new NotFoundError("not found")));

        // Act
        var result = await _controller.GetDocumentAsync("ghost");

        // Assert
        Assert.That(result.Result, Is.TypeOf<NotFoundResult>());
    }

    [Test]
    public async Task AddDocument_ShouldReturnCreated_WhenSuccess()
    {
        // Arrange
        var request = new CreateNewDocumentRequest
        {
            Id = "new1", 
            Tags = [ "t" ], 
            Data = []
        };
        _serviceMock.Setup(s => s.AddAsync(It.IsAny<DocumentDto>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Ok());

        // Act
        var result = await _controller.AddDocumentAsync(request);

        // Assert
        Assert.That(result, Is.TypeOf<CreatedAtActionResult>());
        var created = result as CreatedAtActionResult;
        Assert.That(created.ActionName, Is.EqualTo(nameof(DocumentController.GetDocumentAsync)));
    }

    [Test]
    public async Task AddDocument_ShouldReturnConflict_WhenDuplicate()
    {
        // Arrange
        var request = new CreateNewDocumentRequest
        {
            Id = "dup", 
            Tags = [], 
            Data = []
        };
        _serviceMock.Setup(s => s.AddAsync(It.IsAny<DocumentDto>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Fail(new ConflictError("exists")));

        // Act
        var result = await _controller.AddDocumentAsync(request);

        // Assert
        Assert.That(result, Is.TypeOf<ConflictResult>());
    }

    [Test]
    public async Task DeleteDocument_ShouldReturnNoContent_WhenSuccess()
    {
        // Arrange
        _serviceMock.Setup(s => s.DeleteAsync("d3", It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Ok());

        // Act
        var result = await _controller.DeleteDocumentAsync("d3");

        // Assert
        Assert.That(result, Is.TypeOf<NoContentResult>());
    }

    [Test]
    public async Task DeleteDocument_ShouldReturnNotFound_WhenMissing()
    {
        // Arrange
        _serviceMock.Setup(s => s.DeleteAsync("ghost", It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Fail(new NotFoundError("not found")));

        // Act
        var result = await _controller.DeleteDocumentAsync("ghost");

        // Assert
        Assert.That(result, Is.TypeOf<NotFoundResult>());
    }

    [Test]
    public async Task PatchDocument_ShouldReturnOk_WhenSuccess()
    {
        // Arrange
        var request = new UpdateDocumentRequest
        {
            Id = "patch", 
            NewTags = [ "x" ]
        };
        _serviceMock.Setup(s => s.UpdateAsync(request, It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Ok());

        // Act
        var result = await _controller.PatchDocumentAsync(request);

        // Assert
        Assert.That(result, Is.TypeOf<OkResult>());
    }

    [Test]
    public async Task PatchDocument_ShouldReturnNotFound_WhenMissing()
    {
        // Arrange
        var request = new UpdateDocumentRequest { Id = "ghost" };
        _serviceMock.Setup(s => s.UpdateAsync(request, It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Fail(new NotFoundError("ghost")));

        // Act
        var result = await _controller.PatchDocumentAsync(request);

        // Assert
        Assert.That(result, Is.TypeOf<NotFoundResult>());
    }
}