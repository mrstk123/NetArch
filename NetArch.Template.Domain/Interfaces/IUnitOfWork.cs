#if (IsEFCore)
namespace NetArch.Template.Domain.Interfaces;

public interface IUnitOfWork
{
    Task<int> SaveAsync(CancellationToken cancellationToken = default);
}
#endif
