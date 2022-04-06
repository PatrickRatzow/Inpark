﻿using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Zoo.Common.Api;

public static class DependencyInjection
{
    public static void AddResponseMapper(this IServiceCollection services)
    {
        services.AddScoped<IResponseMapper, DefaultResponseMapper>();
    }

    public static void UseResponseMapper(this IApplicationBuilder app)
    {
    }
}