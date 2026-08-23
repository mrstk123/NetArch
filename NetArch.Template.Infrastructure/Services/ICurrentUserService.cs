#if (IsEFCore || IsHybrid)
namespace NetArch.Template.Infrastructure.Services;

public interface ICurrentUserService
{
    string? UserId { get; }
}
#endif
