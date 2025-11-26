using Hwoodiwiss.Extensions.Hosting.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;
using OpenTelemetry.Logs;

namespace Hwoodiwiss.Extensions.Hosting.Extensions;

internal static class WebApplicationBuilderExtensions
{
    public static WebApplication ConfigureAndBuild(this WebApplicationBuilder builder, ApplicationConfiguration applicationConfiguration)
    {
        bool enableDebugLogging = false;
#if DEBUG
        enableDebugLogging = true;
#endif

        builder.Configuration.ConfigureConfiguration();
        builder.Logging.ConfigureLogging(builder.Configuration, enableDebugLogging);
        builder.Services.ConfigureOptions(builder.Configuration, applicationConfiguration);
        builder.Services.ConfigureHttpClients();
        builder.Services.ConfigureServices(builder.Configuration, applicationConfiguration);

        return builder.Build();
    }
}
