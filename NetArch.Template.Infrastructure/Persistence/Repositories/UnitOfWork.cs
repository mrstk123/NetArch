#if (IsEFCore)
#if (IsClean)
using NetArch.Template.Domain.Interfaces;
#endif
#if (IsNTier)
using NetArch.Template.BusinessLogic.Interfaces;
#endif
using NetArch.Template.Infrastructure.Persistence.Context;

namespace NetArch.Template.Infrastructure.Persistence.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly AppDbContext _context;

    public UnitOfWork(AppDbContext context)
    {
        _context = context;
    }

    public async Task<int> SaveAsync(CancellationToken cancellationToken = default)
        => await _context.SaveChangesAsync(cancellationToken);
}
#endif
