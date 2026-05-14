using BenchmarkDotNet.Attributes;
using Microsoft.AspNetCore.Mvc.Testing;

namespace HwoodiwissApplication.Benchmarks;

[MemoryDiagnoser]
public class HwoodiwissApplicationBenchmarks
{
    private static readonly Uri HealthPath = new("/health", UriKind.Relative);
    private static readonly Uri ConfigurationPath = new("/configuration/version", UriKind.Relative);
    private static readonly Uri WeatherForecastPath = new("/weatherforecast", UriKind.Relative);

    private WebApplicationFactory<Program> _factory = null!;
    private HttpClient _client = null!;

    [GlobalSetup]
    public void Setup()
    {
        _factory = new WebApplicationFactory<Program>();
        _client = _factory.CreateClient();
    }

    [GlobalCleanup]
    public void Cleanup()
    {
        _client.Dispose();
        _factory.Dispose();
    }

    [Benchmark]
    public async Task<HttpResponseMessage> Health()
        => await _client.GetAsync(HealthPath).ConfigureAwait(false);

    [Benchmark]
    public async Task<HttpResponseMessage> Configuration()
        => await _client.GetAsync(ConfigurationPath).ConfigureAwait(false);

    [Benchmark]
    public async Task<HttpResponseMessage> WeatherForecast()
        => await _client.GetAsync(WeatherForecastPath).ConfigureAwait(false);
}
