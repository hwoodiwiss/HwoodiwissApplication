using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Hwoodiwiss.Extensions.Hosting.Endpoints;

public static class HealthcheckEndpoints
{
    public static IEndpointRouteBuilder MapHealthEndpoints(this IEndpointRouteBuilder builder)
    {
        var group = builder.MapGroup("/health")
                .ExcludeFromDescription();

        group.MapGet("/", () => Results.Ok());

        return builder;
    }
}
