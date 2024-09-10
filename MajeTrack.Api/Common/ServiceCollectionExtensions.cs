using System.Reflection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace MajeTrack.Api.Common;

internal static class ServiceCollectionExtensions
{
    /// <summary>
    /// Add <see cref="IEndpoint"/> implementation endpoints to <paramref name="services"/> in transient scope.
    /// </summary>
    /// <param name="services">Services to which the endpoints will be added</param>
    /// <param name="assembly">Assembly to get the endpoints from</param>
    /// <returns><see cref="IServiceCollection"/></returns>
    internal static IServiceCollection AddEndpoints(this IServiceCollection services, Assembly assembly)
    {
        var serviceDescriptors = assembly
            .DefinedTypes
            .Where(type => type is { IsAbstract: false, IsInterface: false } && type.IsAssignableTo(typeof(IEndpoint)))
            .Select(type => ServiceDescriptor.Transient(typeof(IEndpoint), type))
            .ToArray();

        services.TryAddEnumerable(serviceDescriptors);

        return services;
    }
    
    /// <summary>
    /// Maps all registered endpoints as Minimal API endpoints
    /// </summary>
    /// <param name="app">App to which the endpoints will be registered</param>
    /// <returns><see cref="IApplicationBuilder"/></returns>
    internal static IApplicationBuilder MapEndpoints(this WebApplication app)
    {
        var endpoints = app.Services.GetRequiredService<IEnumerable<IEndpoint>>();

        foreach (var endpoint in endpoints)
        {
            endpoint.MapEndpoint(app);
        }

        return app;
    }
    
    // TODO: custom AddSwaggerGen vcetne konfigurace a pouziteho Authentication
}