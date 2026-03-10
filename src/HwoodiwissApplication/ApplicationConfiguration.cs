using System.Text.Json.Serialization.Metadata;
using OpenTelemetry.Metrics;
using OpenTelemetry.Trace;

namespace Hwoodiwiss.Extensions.Hosting;

internal sealed class ApplicationConfiguration
{
    public ICollection<IJsonTypeInfoResolver> JsonSerializerContexts { get; } = [];

    public Action<HwoodiwissApplicationOptions>? ConfigureOptions { get; set; }

    public Action<MeterProviderBuilder>? ConfigureMetrics { get; set; }

    public Action<TracerProviderBuilder>? ConfigureTracing { get; set; }
}