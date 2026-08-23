#if (IsClean || IsNTier)
#if (IsClean)
using NetArch.Template.Domain.Entities;
#endif
#if (IsNTier)
using NetArch.Template.BusinessLogic.Entities;
#endif
using Xunit;

namespace NetArch.Template.Tests.Entities;

public class BaseEntityTests
{
    [Fact]
    public void New_entity_has_default_id_and_is_active()
    {
        var entity = new Product();

        Assert.Equal(0, entity.Id);
        Assert.True(entity.IsActive);
        Assert.Equal(default, entity.CreatedAt);
        Assert.Null(entity.CreatedBy);
    }

    [Fact]
    public void Product_defaults_name_to_empty_string()
    {
        var product = new Product();

        Assert.Equal(string.Empty, product.Name);
        Assert.Equal(0m, product.Price);
    }
}
#endif
