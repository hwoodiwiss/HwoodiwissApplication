namespace Hwoodiwiss.Extensions.Hosting;

public sealed class HwoodiwissApplicationOptions
{
    public IList<string> BlockedUserAgents { get; } = [];

    public bool HostStaticAssets { get; set; }

    public bool DisableDefaultEndpoints { get; set; }
}