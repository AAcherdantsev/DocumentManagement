using DocumentManagement.Models.Documents;
using DocumentManagement.Persistence.DocumentRepositories;
using DocumentManagement.PublicModels.Errors;

namespace DocumentManagement.Persistence.Tests.DocumentRepositories;

[TestFixture]
public class InMemoryDocumentRepositoryTests
{
    private InMemoryDocumentRepository _repo;

    [SetUp]
    public void Setup()
    {
        _repo = new InMemoryDocumentRepository();
    }

    [Test]
    public async Task AddAsync_ShouldAddDocument_WhenNew()
    {
        // Arrange
        var doc = new Document { Id = "doc1", Data = [], Tags = [] };

        // Act
        var result = await _repo.AddAsync(doc);

        // Assert
        Assert.That(result.IsSuccess, Is.True);
        var getResult = await _repo.GetAsync("doc1");
        Assert.That(getResult.IsSuccess, Is.True);
        Assert.That(getResult.Value, Is.SameAs(doc));
    }

    [Test]
    public async Task AddAsync_ShouldFail_WhenDuplicateId()
    {
        // Arrange
        var doc = new Document { Id = "doc2", Data = [], Tags = [] };
        
        // Act
        await _repo.AddAsync(doc);
        var result = await _repo.AddAsync(doc);

        // Assert
        Assert.That(result.IsFailed, Is.True);
        Assert.That(result.Errors, Has.One.InstanceOf<ConflictError>());
    }

    [Test]
    public async Task GetAsync_ShouldFail_WhenNotExists()
    {
        // Act
        var result = await _repo.GetAsync("nope");

        // Assert
        Assert.That(result.IsFailed, Is.True);
        Assert.That(result.Errors, Has.One.InstanceOf<NotFoundError>());
    }

    [Test]
    public async Task UpdateAsync_ShouldUpdate_WhenExists()
    {
        // Arrange & Act
        var doc = new Document { Id = "doc3", Data = [], Tags = [] };
        await _repo.AddAsync(doc);

        var updated = new Document { Id = "doc3", Tags = ["updated"], Data = []};
        var result = await _repo.UpdateAsync("doc3", updated);

        // Assert
        Assert.That(result.IsSuccess, Is.True);
        var getResult = await _repo.GetAsync("doc3");
        Assert.That(getResult.Value.Tags, Does.Contain("updated"));
    }

    [Test]
    public async Task UpdateAsync_ShouldFail_WhenNotExists()
    {
        // Arrange
        var updated = new Document { Id = "ghost", Data = [], Tags = [] };
        
        // Act
        var result = await _repo.UpdateAsync("ghost", updated);

        // Assert
        Assert.That(result.IsFailed, Is.True);
        Assert.That(result.Errors, Has.One.InstanceOf<NotFoundError>());
    }

    [Test]
    public async Task DeleteAsync_ShouldRemove_WhenExists()
    {
        // Arrange
        var doc = new Document { Id = "doc4", Data = [], Tags = [] };
        
        // Act
        await _repo.AddAsync(doc);
        var result = await _repo.DeleteAsync("doc4");

        // Assert
        Assert.That(result.IsSuccess, Is.True);
        var getResult = await _repo.GetAsync("doc4");
        Assert.That(getResult.IsFailed, Is.True);
    }

    [Test]
    public async Task DeleteAsync_ShouldFail_WhenNotExists()
    {
        // Act
        var result = await _repo.DeleteAsync("ghost");

        // Assert
        Assert.That(result.IsFailed, Is.True);
        Assert.That(result.Errors, Has.One.InstanceOf<NotFoundError>());
    }
}