using AutoMapper;
using DocumentManagement.API.Requests;
using DocumentManagement.API.Services.Documents.Implementations;
using DocumentManagement.Models.Documents;
using DocumentManagement.Persistence.DocumentRepositories;
using DocumentManagement.PublicModels.Documents;
using DocumentManagement.PublicModels.Errors;
using FluentResults;
using Moq;

namespace DocumentManagement.API.Tests.Services.Documents.Implementations;

[TestFixture]
public class DocumentServiceTests
{
    private Mock<IDocumentRepository> _repoMock;
    private IMapper _mapper;
    private DocumentService _service;

    [SetUp]
    public void Setup()
    {
        _repoMock = new Mock<IDocumentRepository>();
        var config = new MapperConfiguration(cfg => { cfg.CreateMap<DocumentDto, Document>().ReverseMap(); });
        _mapper = config.CreateMapper();
        _service = new DocumentService(_repoMock.Object, _mapper);
    }

    [Test]
    public async Task GetAsync_ShouldReturnMappedDto_WhenDocumentExists()
    {
        // Arrange
        var doc = new Document
        {
            Id = "doc1", 
            Tags = [ "tag" ], 
            Data = new Dictionary<string, string>() { ["old"] = "data" }
        };
        _repoMock.Setup(r => r.GetAsync("doc1", It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Ok(doc));

        // Act
        var result = await _service.GetAsync("doc1");

        // Assert
        Assert.That(result.IsSuccess, Is.True);
        Assert.That(result.Value.Id, Is.EqualTo("doc1"));
        Assert.That(result.Value.Tags, Does.Contain("tag"));
    }

    [Test]
    public async Task GetAsync_ShouldReturnFail_WhenNotFound()
    {
        // Arrange
        _repoMock.Setup(r => r.GetAsync("ghost", It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Fail<Document>(new NotFoundError("Not found")));

        // Act
        var result = await _service.GetAsync("ghost");

        // Assert
        Assert.That(result.IsFailed, Is.True);
        Assert.That(result.Errors, Has.One.InstanceOf<NotFoundError>());
    }

    [Test]
    public async Task AddAsync_ShouldSetTimestamps_AndCallRepository()
    {
        // Arrange
        var dto = new DocumentDto { Id = "newdoc", Tags = [ "t" ], Data = [] };
        Document? savedDoc = null;

        _repoMock.Setup(r => r.AddAsync(It.IsAny<Document>(), It.IsAny<CancellationToken>()))
            .Callback<Document, CancellationToken>((d, _) => savedDoc = d)
            .ReturnsAsync(Result.Ok());

        // Act
        var result = await _service.AddAsync(dto);

        // Assert
        Assert.That(result.IsSuccess, Is.True);
        Assert.That(savedDoc, Is.Not.Null);
        Assert.That(savedDoc.Created, Is.Not.EqualTo(default(DateTime)));
        Assert.That(savedDoc.LastUpdated, Is.Not.EqualTo(default(DateTime)));
    }

    [Test]
    public async Task UpdateAsync_ById_ShouldCallRepository()
    {
        // Arrange
        var dto = new DocumentDto
        {
            Id = "u1", 
            Tags = [ "x" ], 
            Data =  new Dictionary<string, string>() { ["k"] = "v" }
        };
        
        _repoMock.Setup(r => r.UpdateAsync(dto.Id, It.IsAny<Document>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Ok());

        // Act
        var result = await _service.UpdateAsync(dto.Id, dto);

        // Assert
        Assert.That(result.IsSuccess, Is.True);
        _repoMock.Verify(r => r.UpdateAsync(dto.Id, It.IsAny<Document>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Test]
    public async Task UpdateAsync_ByRequest_ShouldUpdateDataAndTags_WhenExists()
    {
        // Arrange
        var doc = new Document
        {
            Id = "d1", 
            Tags = [ "old" ], 
            Data = new Dictionary<string, string>() { ["old"] = "data" }
        };
        _repoMock.Setup(r => r.GetAsync(doc.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Ok(doc));
        _repoMock.Setup(r => r.UpdateAsync(doc.Id, It.IsAny<Document>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Ok());
        
        var request = new UpdateDocumentRequest
        {
            Id = "d1",
            NewTags = [ "new" ],
            NewData = new Dictionary<string, string>() { ["old"] = "val" }
        };
        
        // Act
        var result = await _service.UpdateAsync(request);

        // Assert
        Assert.That(result.IsSuccess, Is.True);
        Assert.That(doc.Tags, Does.Contain("new"));
        Assert.That(doc.Data, Is.EqualTo(request.NewData));
    }

    [Test]
    public async Task UpdateAsync_ByRequest_ShouldFail_WhenNotFound()
    {
        // Arrange
        _repoMock.Setup(r => r.GetAsync("nope", It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Fail<Document>(new NotFoundError("nope")));

        var request = new UpdateDocumentRequest { Id = "nope" };

        // Act
        var result = await _service.UpdateAsync(request);

        // Assert
        Assert.That(result.IsFailed, Is.True);
        Assert.That(result.Errors, Has.One.InstanceOf<NotFoundError>());
    }

    [Test]
    public async Task DeleteAsync_ShouldCallRepository()
    {
        // Arrange
        _repoMock.Setup(r => r.DeleteAsync("d2", It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Ok());

        // Act
        var result = await _service.DeleteAsync("d2");

        // Assert
        Assert.That(result.IsSuccess, Is.True);
        _repoMock.Verify(r => r.DeleteAsync("d2", It.IsAny<CancellationToken>()), Times.Once);
    }
}
