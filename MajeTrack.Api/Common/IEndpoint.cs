namespace MajeTrack.Api.Common;

public interface IEndpoint
{
    /// <summary>
    /// Maps the endpoint as a Minimal API endpoint
    /// </summary>
    void MapEndpoint(IEndpointRouteBuilder app);
}
