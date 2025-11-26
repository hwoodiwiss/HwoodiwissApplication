using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Hwoodiwiss.Extensions.Hosting.Middleware;

#pragma warning disable CA1812 // Internal class is instantiated by the DI container
internal sealed partial class UserAgentBlockMiddleware : IMiddleware
{
    private readonly ILogger<UserAgentBlockMiddleware> _logger;
    private HwoodiwissApplicationOptions _configuration;
    private readonly IDisposable? _configurationSubscription;

    public UserAgentBlockMiddleware(ILogger<UserAgentBlockMiddleware> logger, IOptionsMonitor<HwoodiwissApplicationOptions> configuration)
    {
        _logger = logger;
        _configuration = configuration.CurrentValue;
        _configurationSubscription = configuration.OnChange(config => _configuration = config);
    }

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        var userAgent = context.Request.Headers.UserAgent.ToString();
        var disallowedUaParts = _configuration.BlockedUserAgents;
        if (disallowedUaParts is not null && ContainsAny(userAgent, disallowedUaParts))
        {
            Log.BlockedUserAgent(_logger, userAgent);
            context.Response.StatusCode = StatusCodes.Status404NotFound;
            return;
        }

        await next(context).ConfigureAwait(false);
    }

    private static bool ContainsAny(string userAgent, IEnumerable<string> disallowedItems)
    {
        foreach (var item in disallowedItems)
        {
            if (userAgent.Contains(item, StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }
        }

        return false;
    }

    public void Dispose()
    {
        _configurationSubscription?.Dispose();
    }

    private static partial class Log
    {
        [LoggerMessage(LogLevel.Information, "Blocked request for user agent: {UserAgent}")]
        public static partial void BlockedUserAgent(ILogger logger, string userAgent);
    }
}
#pragma warning restore