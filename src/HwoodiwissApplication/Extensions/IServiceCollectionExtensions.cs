using System.Net.Http.Headers;
using Hwoodiwiss.Extensions.Hosting.Extensions;
using Hwoodiwiss.Extensions.Hosting.Middleware;
using Microsoft.AspNetCore.Http.Json;
using Microsoft.AspNetCore.HttpLogging;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Hwoodiwiss.Extensions.Hosting.Extensions;

internal static class IServiceCollectionExtensions
{

    private const string HwoodiwissApplicationOptionsSectionName = "HwoodiwissApplication";

    public static IServiceCollection ConfigureOptions(this IServiceCollection services, IConfiguration configuration, ApplicationConfiguration applicationConfiguration)
    {
        services.AddOptions();
        var postConfigureOptions = applicationConfiguration.ConfigureOptions ?? (_ => { });
        services.Configure<HwoodiwissApplicationOptions>(configuration.GetSection(HwoodiwissApplicationOptionsSectionName));
        services.PostConfigure<HwoodiwissApplicationOptions>(postConfigureOptions);
        return services;
    }

    public static IServiceCollection ConfigureHttpClients(this IServiceCollection services)
    {
        services.ConfigureHttpClientDefaults(builder =>
        {
            builder.ConfigureHttpClient(client =>
            {
                client.DefaultRequestHeaders.UserAgent.Add(new ProductInfoHeaderValue(
                    ApplicationMetadata.ApplicationName,
                    ApplicationMetadata.Version));
            });
        });
        services.AddHttpClient();

        return services;
    }

    public static IServiceCollection ConfigureJsonOptions(this IServiceCollection services, Action<JsonOptions> configureOptions)
    {
        services.ConfigureHttpJsonOptions(configureOptions);

        services.Configure<JsonOptions>(Constants.PrettyPrintJsonOptionsKey, options =>
        {
            configureOptions(options);
            options.SerializerOptions.WriteIndented = true;
        });

        services.AddKeyedTransient(KeyedService.AnyKey, (sp, key) =>
        {
            var optionsSnapshot = sp.GetRequiredService<IOptionsSnapshot<JsonOptions>>();
            var jsonOptions = optionsSnapshot.Get(key?.ToString() ?? string.Empty);
            return jsonOptions;
        });

        return services;
    }

    public static IServiceCollection ConfigureServices(this IServiceCollection services, IConfigurationRoot configurationRoot, ApplicationConfiguration applicationOptions)
    {
        services.AddHttpContextAccessor();
        services.ConfigureJsonOptions(options =>
        {
            options.SerializerOptions.TypeInfoResolverChain.Add(DefaultApplicationJsonContext.Default);
            foreach (var jsonContext in applicationOptions.JsonSerializerContexts)
            {
                options.SerializerOptions.TypeInfoResolverChain.Add(jsonContext);
            }
        });

        // Enables easy named loggers in static contexts
        services.AddKeyedTransient(KeyedService.AnyKey, (sp, key) =>
        {
            var loggerFactory = sp.GetRequiredService<ILoggerFactory>();
            return loggerFactory.CreateLogger(key is string keyString ? keyString : "Unknown");
        });

        services.AddTelemetry();

        services.AddMemoryCache();
        services.AddSingleton(configurationRoot);
        services.AddSingleton<UserAgentBlockMiddleware>();
        services.AddHttpLogging(options =>
        {
            options.LoggingFields = HttpLoggingFields.RequestPropertiesAndHeaders | HttpLoggingFields.ResponseStatusCode;
            options.RequestHeaders.Add("X-Forwarded-For");
            options.RequestHeaders.Add("X-Real-IP");
        });

        services.AddOpenApi();

        return services;
    }
}
