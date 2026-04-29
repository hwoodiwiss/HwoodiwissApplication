using System.Diagnostics.CodeAnalysis;
using Hwoodiwiss.Extensions.Hosting.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

namespace Hwoodiwiss.Extensions.Hosting;

public sealed class HwoodiwissApplication : IHost, IApplicationBuilder, IEndpointRouteBuilder, IAsyncDisposable
{
    public static HwoodiwissApplicationBuilder CreateBuilder(string[] args)
    {
        return new HwoodiwissApplicationBuilder(args);
    }

    private readonly WebApplication _webApplication;

    public IServiceProvider ApplicationServices { get => ((IApplicationBuilder)_webApplication).ApplicationServices; set => ((IApplicationBuilder)_webApplication).ApplicationServices = value; }

    public IFeatureCollection ServerFeatures => ((IApplicationBuilder)_webApplication).ServerFeatures;

    public IDictionary<string, object?> Properties => ((IApplicationBuilder)_webApplication).Properties;

    public IServiceProvider Services => _webApplication.Services;

    public IWebHostEnvironment Environment => _webApplication.Environment;

    public IServiceProvider ServiceProvider => ((IEndpointRouteBuilder)_webApplication).ServiceProvider;

    public ICollection<EndpointDataSource> DataSources => ((IEndpointRouteBuilder)_webApplication).DataSources;

    internal HwoodiwissApplication(WebApplication webApplication)
    {
        _webApplication = webApplication;
        var applicationOptions = _webApplication.Services.GetRequiredService<IOptions<HwoodiwissApplicationOptions>>();
        _webApplication.ConfigureRequestPipeline(applicationOptions.Value);
    }

#pragma warning disable CA1054 // Parameter needs to match the params on the WebApplication type
    public Task RunAsync([StringSyntax(StringSyntaxAttribute.Uri)] string? url = null)
#pragma warning restore CA1054
    {
        return _webApplication.RunAsync(url);
    }

#pragma warning disable CA1054 // Parameter needs to match the params on the WebApplication type
    public void Run([StringSyntax(StringSyntaxAttribute.Uri)] string? url = null)
#pragma warning restore CA1054
    {
        _webApplication.Run(url);
    }

    public IApplicationBuilder Use(Func<RequestDelegate, RequestDelegate> middleware)
    {
        _webApplication.Use(middleware);
        return this;
    }

    public IApplicationBuilder New()
    {
        ((IApplicationBuilder)_webApplication).New();
        return this;
    }

    public RequestDelegate Build()
    {
        throw new NotImplementedException();
    }

    public ValueTask DisposeAsync()
    {
        return _webApplication.DisposeAsync();
    }

    public Task StartAsync(CancellationToken cancellationToken = default)
    {
        return _webApplication.StartAsync(cancellationToken);
    }

    public Task StopAsync(CancellationToken cancellationToken = default)
    {
        return _webApplication.StopAsync(cancellationToken);
    }

    public void Dispose()
    {
        ((IDisposable)_webApplication).Dispose();
    }

    public IApplicationBuilder CreateApplicationBuilder()
    {
        return ((IEndpointRouteBuilder)_webApplication).CreateApplicationBuilder();
    }
}