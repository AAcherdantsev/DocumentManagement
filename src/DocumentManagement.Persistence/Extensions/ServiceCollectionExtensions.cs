using DocumentManagement.API.Services.Documents;
using DocumentManagement.Persistence.DocumentRepositories;
using Microsoft.Extensions.DependencyInjection;

namespace DocumentManagement.Persistence.Extensions;

/// <summary>
/// Provides extension methods for configuring services related to document management in the application.
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Registers an in-memory document repository into the service collection.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <returns>The updated service collection.</returns>
    public static IServiceCollection AddInMemoryDocumentRepository(this IServiceCollection services)
    {
        services.AddSingleton<IDocumentRepository, InMemoryDocumentRepository>();
        return services;
    }
}