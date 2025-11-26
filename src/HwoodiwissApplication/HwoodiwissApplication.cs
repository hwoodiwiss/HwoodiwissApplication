using System.Diagnostics.CodeAnalysis;
using Hwoodiwiss.Extensions.Hosting.Extensions;
using Microsoft.AspNetCore.Builder;

namespace Hwoodiwiss.Extensions.Hosting;

public sealed class HwoodiwissApplication
{
    public static HwoodiwissApplicationBuilder CreateBuilder(string[] args)
    {
        return new HwoodiwissApplicationBuilder(args);
    }

    private readonly WebApplication _webApplication;

    internal HwoodiwissApplication(WebApplication webApplication)
    {
        _webApplication = webApplication;
        _webApplication.ConfigureRequestPipeline();
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
}