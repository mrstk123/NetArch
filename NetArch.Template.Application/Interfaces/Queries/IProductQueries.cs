using NetArch.Template.Application.DTOs;

namespace NetArch.Template.Application.Interfaces.Queries;

public interface IProductQueries
{
    Task<IEnumerable<ProductSummaryDto>> GetActiveAsync(CancellationToken cancellationToken = default);

    Task<ProductSummaryDto?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
}
