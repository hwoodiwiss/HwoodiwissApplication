using Hwoodiwiss.Extensions.Hosting.Infrastructure.Filters;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace Hwoodiwiss.Extensions.Hosting.Extensions;

public static class IEndpointBuilderExtensions
{
    public static T WithPrettyPrint<T>(this T builder)
        where T : IEndpointConventionBuilder
    {
        builder.AddEndpointFilterFactory(PrettyPrintJson.Factory);

        return builder;
    }
}
