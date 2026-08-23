using NetArch.Template.BusinessLogic.DTOs;

namespace NetArch.Template.BusinessLogic.Interfaces.Queries;

public interface IProductQueries
{
    Task<IEnumerable<ProductSummaryDto>> GetActiveAsync(CancellationToken cancellationToken = default);

    Task<ProductSummaryDto?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
}
