#if (IsEFCore || IsHybrid)
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
#if (IsClean)
using NetArch.Template.Domain.Entities;
#endif
#if (IsNTier)
using NetArch.Template.BusinessLogic.Entities;
#endif

namespace NetArch.Template.Infrastructure.Persistence.Configurations;

public class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.ToTable("Products");

        builder.HasKey(p => p.Id);

        builder.Property(p => p.Name)
            .HasMaxLength(200)
            .IsRequired();

        builder.Property(p => p.Price)
            .HasPrecision(18, 2);
    }
}
#endif
