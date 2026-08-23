#if (IsClean || IsNTier)
#if (IsClean)
using NetArch.Template.Application.DTOs;
using NetArch.Template.Application.Interfaces.Queries;
#endif
#if (IsNTier)
using NetArch.Template.BusinessLogic.DTOs;
using NetArch.Template.BusinessLogic.Interfaces.Queries;
#endif
using System.Data;
using Dapper;

namespace NetArch.Template.Infrastructure.Queries;

public class ProductQueries : IProductQueries
{
    private readonly IDbConnection _connection;

    public ProductQueries(IDbConnection connection)
    {
        _connection = connection;
    }

    public async Task<IEnumerable<ProductSummaryDto>> GetActiveAsync(CancellationToken cancellationToken = default)
    {
        var command = new CommandDefinition(
            "SELECT Id, Name FROM Products WHERE IsActive = 1 ORDER BY Name",
            cancellationToken: cancellationToken);

        return await _connection.QueryAsync<ProductSummaryDto>(command);
    }

    public async Task<ProductSummaryDto?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var command = new CommandDefinition(
            "SELECT TOP 1 Id, Name FROM Products WHERE Id = @Id AND IsActive = 1",
            new { Id = id },
            cancellationToken: cancellationToken);

        return await _connection.QuerySingleOrDefaultAsync<ProductSummaryDto>(command);
    }
}
#endif
