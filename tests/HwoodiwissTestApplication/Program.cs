using System.Security.Cryptography;
using System.Text.Json.Serialization;
using Hwoodiwiss.Extensions.Hosting;

var builder = HwoodiwissApplication.CreateBuilder(args);
builder.ConfigureOptions(options =>
{
    options.BlockedUserAgents.Add("BadBot");
});
builder.WithHttpJsonContexts(WeatherForecastJsonContext.Default);

var app = builder.Build();

var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

app.MapGet("/weatherforecast", () =>
{
    static int GetSecureRandomInt(int minValue, int maxValue)
    {
        var diff = maxValue - minValue;
        var uint32Buffer = new byte[4];
        RandomNumberGenerator.Fill(uint32Buffer);
        var rand = BitConverter.ToUInt32(uint32Buffer, 0);
        return (int)(minValue + (rand % diff));
    }

    var forecast = Enumerable.Range(1, 5).Select(index =>
        new WeatherForecast
        (
            DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            GetSecureRandomInt(-20, 55),
            summaries[GetSecureRandomInt(0, summaries.Length)]
        ))
        .ToArray();
    return forecast;
})
.WithName("GetWeatherForecast");

app.Run();

#if !NET10_0_OR_GREATER
public partial class Program { }
#endif

sealed record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}

[JsonSerializable(typeof(WeatherForecast))]
[JsonSerializable(typeof(WeatherForecast[]))]
sealed partial class WeatherForecastJsonContext : JsonSerializerContext;
