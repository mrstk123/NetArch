#if (IsEFCore || IsHybrid)
using Microsoft.EntityFrameworkCore;
#endif
#if (IsDapper || IsHybrid)
using System.Data;
using Microsoft.Data.SqlClient;
#endif
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
#if (IsClean)
using NetArch.Template.Domain.Interfaces;
#endif
#if (IsNTier)
using NetArch.Template.BusinessLogic.Interfaces;
#endif

#if (IsEFCore || IsHybrid)
using NetArch.Template.Infrastructure.Persistence.Context;
#endif
#if (IsEFCore)
using NetArch.Template.Infrastructure.Persistence.Interceptors;
using NetArch.Template.Infrastructure.Persistence.Repositories;
#endif

namespace NetArch.Template.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(
        this IServiceCollection services,
        IConfiguration configuration)
    {
#if (IsEFCore || IsHybrid)
#if (IsClean && IsEFCore)
        services.AddScoped<AuditableEntityInterceptor>();
#endif

        services.AddDbContext<AppDbContext>((sp, options) =>
        {
            options.UseSqlServer(
                configuration.GetConnectionString("DefaultConnection"),
                b => b.MigrationsAssembly(typeof(AppDbContext).Assembly.FullName));

#if (IsClean && IsEFCore)
            var interceptor = sp.GetRequiredService<AuditableEntityInterceptor>();
            options.AddInterceptors(interceptor);
#endif
        });
#endif

#if (IsDapper || IsHybrid)
        services.AddScoped<IDbConnection>(_ =>
            new SqlConnection(configuration.GetConnectionString("DefaultConnection")));
#endif

#if (IsEFCore)
        services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
        services.AddScoped<IUnitOfWork, UnitOfWork>();
#endif

        return services;
    }
}
