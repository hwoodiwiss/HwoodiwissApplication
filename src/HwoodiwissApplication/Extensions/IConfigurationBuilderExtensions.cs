using Microsoft.Extensions.Configuration;

namespace Hwoodiwiss.Extensions.Hosting.Extensions;

public static class IConfigurationBuilderExtensions
{
    public static IConfigurationBuilder ConfigureConfiguration(this IConfigurationBuilder configurationBuilder) =>
        configurationBuilder
            .AddUserSecrets(ApplicationMetadata.ApplicationAssembly);
}