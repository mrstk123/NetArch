using System;
using Microsoft.Extensions.DependencyInjection;

namespace NetArch.Template.BusinessLogic;

public static class DependencyInjection
{
    public static IServiceCollection AddBusinessLogicServices(this IServiceCollection services)
    {
        // Register specific business logic services here

        return services;
    }
}
