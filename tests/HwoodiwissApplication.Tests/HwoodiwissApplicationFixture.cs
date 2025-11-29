using System.Diagnostics;
using Microsoft.AspNetCore.Mvc.Testing;
using TUnit.Core.Interfaces;

namespace HwoodiwissApplication.Tests;

public sealed class HwoodiwissApplicationFixture : WebApplicationFactory<Program>, IHwoodiwissTestApplication
{
    private readonly HttpClient _client;

    public HwoodiwissApplicationFixture()
    {
        _client = CreateClient();
    }

    public async Task<HttpResponseMessage> GetForecasts(Action<HttpClient>? configureClient = null)
    {
        Debug.Assert(_client is not null, "_client is not null");
        configureClient?.Invoke(_client);
        return await _client.GetAsync(new Uri("/weatherforecast", UriKind.Relative));
    }

    public override ValueTask DisposeAsync()
    {
        _client?.Dispose();
        return base.DisposeAsync();
    }
}