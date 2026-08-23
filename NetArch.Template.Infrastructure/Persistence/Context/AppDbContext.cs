#if (IsEFCore || IsHybrid)
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
#if (IsClean)
using NetArch.Template.Domain.Entities;
#endif
#if (IsNTier)
using NetArch.Template.BusinessLogic.Entities;
#endif

namespace NetArch.Template.Infrastructure.Persistence.Context;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
        ApplySoftDeleteQueryFilters(modelBuilder);
        base.OnModelCreating(modelBuilder);
    }

    private static void ApplySoftDeleteQueryFilters(ModelBuilder modelBuilder)
    {
        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            if (!typeof(BaseEntity).IsAssignableFrom(entityType.ClrType)) continue;

            var parameter = Expression.Parameter(entityType.ClrType);
            var isActive = Expression.Equal(
                Expression.Property(parameter, nameof(BaseEntity.IsActive)),
                Expression.Constant(true));
            var filter = Expression.Lambda(isActive, parameter);

            modelBuilder.Entity(entityType.ClrType).HasQueryFilter(filter);
        }
    }
}
#endif
