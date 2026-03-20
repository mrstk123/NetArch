using System;

namespace NetArch.Template.Domain.Interfaces;

public interface IUnitOfWork: IDisposable
{

    Task<int> SaveAsync(CancellationToken cancellationToken = default);
}
