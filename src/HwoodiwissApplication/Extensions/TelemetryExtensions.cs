using System.Runtime.CompilerServices;
using Microsoft.Extensions.DependencyInjection;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

namespace Hwoodiwiss.Extensions.Hosting.Extensions;

internal static class TelemetryExtensions
{
    public static IServiceCollection AddTelemetry(this IServiceCollection services)
    {
        services.AddOpenTelemetry()
            .ConfigureResource(TelemetryResourceBuilder)
            .WithMetrics(metrics =>
            {
                metrics.AddRuntimeInstrumentation()
                    .AddAspNetCoreInstrumentation()
                    .AddHttpClientInstrumentation()
                    .AddMeter("Microsoft.AspNetCore.Hosting")
                    .AddMeter("Microsoft.AspNetCore.Server.Kestrel")
                    .AddMeter("Microsoft.AspNetCore.Diagnostics")
                    .AddOtlpExporter();
            })
            .WithTracing(tracing =>
            {
                tracing.AddAspNetCoreInstrumentation()
                    .AddHttpClientInstrumentation()
                    .AddOtlpExporter();
            });

        static void TelemetryResourceBuilder(ResourceBuilder resourceBuilder)
        {
            var resourceAttributes = new List<KeyValuePair<string, object>>();
            AddAttributeIfPresent("service.version", ApplicationMetadata.Version);
            AddAttributeIfPresent("service.commit", ApplicationMetadata.GitCommit);
            AddAttributeIfPresent("service.branch", ApplicationMetadata.GitBranch);
            AddAttributeIfPresent("service.host", Environment.MachineName);
            AddAttributeIfPresent("service.aot", !(RuntimeFeature.IsDynamicCodeSupported & RuntimeFeature.IsDynamicCodeCompiled));

            resourceBuilder
                .AddService(ApplicationMetadata.ApplicationName ?? throw new InvalidOperationException("Application name is not available for telemetry."))
                .AddAttributes(resourceAttributes)
                .AddContainerDetector()
                .AddHostDetector();

            void AddAttributeIfPresent(string key, object? value)
            {
                if (value is not null)
                {
                    resourceAttributes.Add(new KeyValuePair<string, object>(key, value));
                }
            }
        }

        return services;
    }
}
