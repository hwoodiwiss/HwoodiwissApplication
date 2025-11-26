using System.Reflection;

namespace Hwoodiwiss.Extensions.Hosting;

internal static class ApplicationMetadata
{
    public static string ApplicationName => ApplicationAssembly.GetName().Name ?? throw new InvalidOperationException("Unable to determine application name.");

    public static string? Version => GetVersion();

    public static string? GitBranch => GetCustomMetadata("GitBranch");

    public static string? GitCommit => GetCustomMetadata("GitCommit");

    private static string? GetVersion() => ApplicationAssembly.GetCustomAttribute<AssemblyInformationalVersionAttribute>()?.InformationalVersion;

    private static string? GetCustomMetadata(string key) => AssemblyMetadata.TryGetValue(key, out var value) ? value : null;

    private static Dictionary<string, string?> AssemblyMetadata => ApplicationAssembly.GetCustomAttributes<AssemblyMetadataAttribute>()
        .ToDictionary(k => k.Key, v => v.Value, StringComparer.OrdinalIgnoreCase);

    public static Assembly ApplicationAssembly => Assembly.GetEntryAssembly()
        ?? throw new InvalidOperationException("This library does not support being loaded in an unmanaged context.");
}