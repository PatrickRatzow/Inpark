using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Zeta.Common.Api;

public static class ResponseMapperDependencyInjection
{
    public static void AddResponseMapper(this IServiceCollection services)
    {
        services.TryAddScoped<IResponseMapper, DefaultResponseMapper>();
    }

    public static void UseResponseMapper(this IApplicationBuilder app)
    {
    }
}