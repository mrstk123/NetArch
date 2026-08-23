#if (IsClean || IsNTier)
#if (IsClean)
using NetArch.Template.Application.DTOs;
using NetArch.Template.Application.Interfaces.Commands;
#endif
#if (IsNTier)
using NetArch.Template.BusinessLogic.DTOs;
using NetArch.Template.BusinessLogic.Interfaces.Commands;
#endif
using System.Data;
using Dapper;

namespace NetArch.Template.Infrastructure.Commands;

public class ProductCommands : IProductCommands
{
    private const string InsertSql =
        """
        INSERT INTO Products (Name, IsActive, CreatedAt, UpdatedAt)
        VALUES (@Name, 1, SYSUTCDATETIME(), SYSUTCDATETIME());
        SELECT CAST(SCOPE_IDENTITY() AS INT);
        """;

    private readonly IDbConnection _connection;

    public ProductCommands(IDbConnection connection)
    {
        _connection = connection;
    }

    public async Task<int> CreateAsync(CreateProductRequest request, CancellationToken cancellationToken = default)
    {
        var command = new CommandDefinition(InsertSql, request, cancellationToken: cancellationToken);
        return await _connection.ExecuteScalarAsync<int>(command);
    }
}
#endif
