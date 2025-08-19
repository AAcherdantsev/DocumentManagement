using DocumentManagement.Persistence.DocumentRepositories;
using DocumentManagement.Persistence.Extensions;
using Microsoft.Extensions.DependencyInjection;
using Assert = NUnit.Framework.Assert;

namespace DocumentManagement.Persistence.Tests.Extensions;

public class ServiceCollectionExtensionsTests
{
    [Test]
    public void AddInMemoryDocumentRepository_RegistersInMemoryImplementation()
    {
        // Arrange
        var services = new ServiceCollection();

        // Act
        services.AddInMemoryDocumentRepository();
        var provider = services.BuildServiceProvider();
        var repo = provider.GetService<IDocumentRepository>();

        // Assert
        Assert.NotNull(repo);
        Assert.That(repo, Is.InstanceOf<InMemoryDocumentRepository>());
    }

    [Test]
    public void AddInMemoryDocumentRepository_ReturnsSameInstance_OnMultipleResolves()
    {
        // Arrange
        var services = new ServiceCollection();
        services.AddInMemoryDocumentRepository();
        var provider = services.BuildServiceProvider();

        // Act
        var repo1 = provider.GetService<IDocumentRepository>();
        var repo2 = provider.GetService<IDocumentRepository>();

        // Assert
        Assert.That(repo1, Is.SameAs(repo2));
    }
}