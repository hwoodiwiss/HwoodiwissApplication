
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;
using OpenTelemetry.Logs;

namespace Hwoodiwiss.Extensions.Hosting.Extensions;

internal static class ILoggingBuilderExtensions
{
    internal static ILoggingBuilder ConfigureLogging(this ILoggingBuilder builder, IConfiguration? configuration = null, bool enableDebugLogging = false)
    {
        if (configuration is not null)
        {
            builder.AddConfiguration(configuration);
        }

        builder.AddOpenTelemetry(opt =>
            {
                opt.IncludeScopes = true;
                opt.IncludeFormattedMessage = true;
                opt.AddOtlpExporter();
            });

        if (enableDebugLogging)
        {
            builder
                .AddConsole()
                .AddDebug();

            builder.Services.Configure<ConsoleFormatterOptions>(options =>
            {
                options.IncludeScopes = true;
            });
        }

        return builder;
    }
}