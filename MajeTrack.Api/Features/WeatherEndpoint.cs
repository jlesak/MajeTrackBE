using MajeTrack.Api.Common;

namespace MajeTrack.Api.Features;

internal sealed class WeatherEndpoint : IEndpoint
{
    /// <inheritdoc />
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("/weatherforecast", HandleAsync)
            .WithName("GetWeatherForecast")
            .WithOpenApi();;
    }

    private static Task<IResult> HandleAsync([AsParameters] WeatherRequest request)
    {
        var summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };
        
        var forecast = Enumerable.Range(1, 5).Select(index =>
                new WeatherForecast
                (
                    request.City,
                    DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                    Random.Shared.Next(-20, 55),
                    summaries[Random.Shared.Next(summaries.Length)]
                ))
            .ToArray();
        
        return Task.FromResult<IResult>(TypedResults.Ok(forecast));
    }
}


public record WeatherRequest(string City);

public record WeatherForecast(string City, DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
