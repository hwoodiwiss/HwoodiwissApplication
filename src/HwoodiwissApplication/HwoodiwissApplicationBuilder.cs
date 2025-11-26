using System.Text.Json.Serialization;
using Hwoodiwiss.Extensions.Hosting.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Hwoodiwiss.Extensions.Hosting;

public sealed partial class HwoodiwissApplicationBuilder
{
    private readonly WebApplicationBuilder _webApplicationBuilder;
    private readonly ApplicationConfiguration _applicationConfiguration = new();
    private readonly ILoggerFactory _loggerFactory;
    private readonly ILogger<HwoodiwissApplicationBuilder> _startupLogger;

    public HwoodiwissApplicationBuilder(string[] args)
    {
        _webApplicationBuilder = WebApplication.CreateBuilder(args);
        _loggerFactory = LoggerFactory.Create(builder => builder.ConfigureLogging(null, true));
        _startupLogger = _loggerFactory.CreateLogger<HwoodiwissApplicationBuilder>();
    }

    public IServiceCollection Services => _webApplicationBuilder.Services;

    public ConfigurationManager Configuration => _webApplicationBuilder.Configuration;

    public HwoodiwissApplicationBuilder ConfigureOptions(Action<HwoodiwissApplicationOptions> configureOptions)
    {
        ArgumentNullException.ThrowIfNull(configureOptions);

        _applicationConfiguration.ConfigureOptions = configureOptions;

        return this;
    }

    public HwoodiwissApplicationBuilder WithJsonContexts(params JsonSerializerContext[] jsonTypeInfoResolvers)
    {
        ArgumentNullException.ThrowIfNull(jsonTypeInfoResolvers);

        foreach (var resolver in jsonTypeInfoResolvers)
        {
            _applicationConfiguration.JsonSerializerContexts.Add(resolver);
        }

        return this;
    }

    public HwoodiwissApplication Build()
    {
        try
        {
            Log.StartingBuild(_startupLogger);
            return new HwoodiwissApplication(_webApplicationBuilder.ConfigureAndBuild(_applicationConfiguration));
        }
        catch (Exception ex)
        {
            Log.BuildFailed(_startupLogger, ex);
            throw;
        }
    }

    private static partial class Log
    {
        [LoggerMessage(LogLevel.Trace, "Starting application build...")]
        public static partial void StartingBuild(ILogger logger);

        [LoggerMessage(LogLevel.Critical, "Application configuration failed!")]
        public static partial void BuildFailed(ILogger logger, Exception exception);
    }
}