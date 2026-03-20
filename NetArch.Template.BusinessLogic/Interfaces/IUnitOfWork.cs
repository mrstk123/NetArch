using System;

namespace NetArch.Template.BusinessLogic.Interfaces;

public interface IUnitOfWork : IDisposable
{

    Task<int> SaveAsync(CancellationToken cancellationToken = default);
}
