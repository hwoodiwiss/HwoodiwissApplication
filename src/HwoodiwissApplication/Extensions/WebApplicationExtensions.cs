using Hwoodiwiss.Extensions.Hosting.Endpoints;
using Hwoodiwiss.Extensions.Hosting.Middleware;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Hosting;

namespace Hwoodiwiss.Extensions.Hosting.Extensions;

internal static class WebApplicationExtensions
{
    public static WebApplication ConfigureRequestPipeline(this WebApplication app)
    {
        app.UseMiddleware<UserAgentBlockMiddleware>();
        app.UseDefaultFiles();

        app.UseHttpLogging();
        app.MapEndpoints(app.Environment);

        if (app.Environment.IsDevelopment())
        {
            app.UseStaticFiles();
        }
        else
        {
            app.MapStaticAssets();
            app.MapFallbackToFile("/", "/index.html");
        }

        return app;
    }

    private static IEndpointRouteBuilder MapEndpoints(this IEndpointRouteBuilder builder, IWebHostEnvironment environment)
        => builder
            .MapConfigurationEndpoints(environment)
            .MapHealthEndpoints();
}
