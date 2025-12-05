using System.Text.Json.Serialization;
using Hwoodiwiss.Extensions.Hosting.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.Metrics;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using OpenTelemetry.Metrics;
using OpenTelemetry.Trace;

namespace Hwoodiwiss.Extensions.Hosting;

public sealed partial class HwoodiwissApplicationBuilder : IHostApplicationBuilder
{
    private readonly WebApplicationBuilder _webApplicationBuilder;
    private readonly ApplicationConfiguration _applicationConfiguration = new();
    private readonly ILoggerFactory _loggerFactory;
    private readonly ILogger<HwoodiwissApplicationBuilder> _startupLogger;

    public HwoodiwissApplicationBuilder(string[] args)
    {
        _webApplicationBuilder = WebApplication.CreateSlimBuilder(args);
        _loggerFactory = LoggerFactory.Create(builder => builder.ConfigureLogging(null, true));
        _startupLogger = _loggerFactory.CreateLogger<HwoodiwissApplicationBuilder>();
    }

    public IServiceCollection Services => _webApplicationBuilder.Services;

    public ConfigurationManager Configuration => _webApplicationBuilder.Configuration;

    public IWebHostEnvironment Environment => _webApplicationBuilder.Environment;

    IConfigurationManager IHostApplicationBuilder.Configuration => Configuration;

    IHostEnvironment IHostApplicationBuilder.Environment => Environment;

    public ILoggingBuilder Logging => _webApplicationBuilder.Logging;

    public IMetricsBuilder Metrics => _webApplicationBuilder.Metrics;

    public IDictionary<object, object> Properties => ((IHostApplicationBuilder)_webApplicationBuilder).Properties;


    public HwoodiwissApplicationBuilder ConfigureOptions(Action<HwoodiwissApplicationOptions> configureOptions)
    {
        ArgumentNullException.ThrowIfNull(configureOptions);

        _applicationConfiguration.ConfigureOptions = configureOptions;

        return this;
    }

    public HwoodiwissApplicationBuilder WithHttpJsonContexts(params JsonSerializerContext[] jsonTypeInfoResolvers)
    {
        ArgumentNullException.ThrowIfNull(jsonTypeInfoResolvers);

        foreach (var resolver in jsonTypeInfoResolvers)
        {
            _applicationConfiguration.JsonSerializerContexts.Add(resolver);
        }

        return this;
    }

    public HwoodiwissApplicationBuilder ConfigureMetrics(Action<MeterProviderBuilder> configureMetrics)
    {
        ArgumentNullException.ThrowIfNull(configureMetrics);

        _applicationConfiguration.ConfigureMetrics = configureMetrics;

        return this;
    }

    public HwoodiwissApplicationBuilder ConfigureTracing(Action<TracerProviderBuilder> configureTracing)
    {
        ArgumentNullException.ThrowIfNull(configureTracing);

        _applicationConfiguration.ConfigureTracing = configureTracing;

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

    public void ConfigureContainer<TContainerBuilder>(IServiceProviderFactory<TContainerBuilder> factory, Action<TContainerBuilder>? configure = null) where TContainerBuilder : notnull
    {
        throw new NotImplementedException();
    }

    private static partial class Log
    {
        [LoggerMessage(LogLevel.Trace, "Starting application build...")]
        public static partial void StartingBuild(ILogger logger);

        [LoggerMessage(LogLevel.Critical, "Application configuration failed!")]
        public static partial void BuildFailed(ILogger logger, Exception exception);
    }
}