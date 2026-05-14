using BenchmarkDotNet.Attributes;
using Microsoft.AspNetCore.Mvc.Testing;

namespace HwoodiwissApplication.Benchmarks;

[MemoryDiagnoser]
public sealed class HwoodiwissApplicationBenchmarks
{
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
        => await _client.GetAsync("/health");

    [Benchmark]
    public async Task<HttpResponseMessage> Configuration()
        => await _client.GetAsync("/configuration/version");

    [Benchmark]
    public async Task<HttpResponseMessage> WeatherForecast()
        => await _client.GetAsync("/weatherforecast");
}
