#if (IsEFCore || IsHybrid)
using Microsoft.EntityFrameworkCore;
#endif
using Microsoft.Extensions.Diagnostics.HealthChecks;
#if (IsDapper || IsHybrid)
using System.Data;
using Microsoft.Data.SqlClient;
#endif
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
#if (IsClean && IsEFCore)
using NetArch.Template.Domain.Interfaces;
#endif
#if (IsNTier && IsEFCore)
using NetArch.Template.BusinessLogic.Interfaces;
#endif

#if (IsEFCore || IsHybrid)
using NetArch.Template.Infrastructure.Persistence.Context;
#endif
#if (IsEFCore)
using NetArch.Template.Infrastructure.Persistence.Repositories;
#endif
#if (IsEFCore || IsHybrid)
using NetArch.Template.Infrastructure.Services;
using NetArch.Template.Infrastructure.Persistence.Interceptors;
#endif
#if (IsClean && (IsDapper || IsHybrid))
using NetArch.Template.Application.DTOs;
using NetArch.Template.Application.Interfaces.Commands;
using NetArch.Template.Application.Interfaces.Queries;
#endif
#if (IsNTier && (IsDapper || IsHybrid))
using NetArch.Template.BusinessLogic.DTOs;
using NetArch.Template.BusinessLogic.Interfaces.Commands;
using NetArch.Template.BusinessLogic.Interfaces.Queries;
#endif
#if (IsDapper || IsHybrid)
using NetArch.Template.Infrastructure.Commands;
using NetArch.Template.Infrastructure.Queries;
#endif

namespace NetArch.Template.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var healthChecks = services.AddHealthChecks();

#if (IsEFCore || IsHybrid)
        services.AddHttpContextAccessor();
        services.AddScoped<ICurrentUserService, CurrentUserService>();
        services.AddScoped<AuditableEntityInterceptor>();

        services.AddDbContext<AppDbContext>((sp, options) =>
        {
            options.UseSqlServer(
                configuration.GetConnectionString("DefaultConnection"),
                b => b.MigrationsAssembly(typeof(AppDbContext).Assembly.FullName));

            var interceptor = sp.GetRequiredService<AuditableEntityInterceptor>();
            options.AddInterceptors(interceptor);
        });

        healthChecks.AddDbContextCheck<AppDbContext>("database");
#endif

#if (IsDapper || IsHybrid)
        services.AddScoped<IDbConnection>(_ =>
            new SqlConnection(configuration.GetConnectionString("DefaultConnection")));

        services.AddScoped<IProductQueries, ProductQueries>();
        services.AddScoped<IProductCommands, ProductCommands>();
#endif

#if (IsEFCore)
        services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
        services.AddScoped<IUnitOfWork, UnitOfWork>();
#endif

        return services;
    }
}
