using System.Text.Json.Serialization.Metadata;

namespace Hwoodiwiss.Extensions.Hosting;

internal sealed class ApplicationConfiguration
{
    public ICollection<IJsonTypeInfoResolver> JsonSerializerContexts { get; } = [];

    public Action<HwoodiwissApplicationOptions>? ConfigureOptions { get; set; }
}