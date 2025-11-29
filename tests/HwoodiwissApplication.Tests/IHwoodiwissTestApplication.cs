namespace HwoodiwissApplication.Tests;

public interface IHwoodiwissTestApplication
{
    Task<HttpResponseMessage> GetForecasts(Action<HttpClient>? configureClient = null);
}

public sealed record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}