#if (IsEFCore || IsHybrid)
using Microsoft.EntityFrameworkCore;
#if (IsClean && IsEFCore)
using NetArch.Template.Infrastructure.Persistence.Interceptors;
#endif

namespace NetArch.Template.Infrastructure.Persistence.Context;

public class AppDbContext : DbContext
{
#if (IsClean && IsEFCore)
    private readonly AuditableEntityInterceptor _auditableInterceptor;
#endif

    public AppDbContext(
        DbContextOptions<AppDbContext> options
#if (IsClean && IsEFCore)
        , AuditableEntityInterceptor auditableInterceptor
#endif
    ) : base(options)
    {
#if (IsClean && IsEFCore)
        _auditableInterceptor = auditableInterceptor;
#endif
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
#if (IsClean && IsEFCore)
        optionsBuilder.AddInterceptors(_auditableInterceptor);
#endif
    }
}
#endif
