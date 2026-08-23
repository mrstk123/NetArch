#if (IsEFCore)
namespace NetArch.Template.BusinessLogic.Interfaces;

public interface IUnitOfWork
{
    Task<int> SaveAsync(CancellationToken cancellationToken = default);
}
#endif
