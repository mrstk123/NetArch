using System;
using Microsoft.Extensions.DependencyInjection;

namespace NetArch.Template.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        // Register specific application services here

        return services;
    }

}
