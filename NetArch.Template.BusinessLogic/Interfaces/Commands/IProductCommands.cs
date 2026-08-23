using NetArch.Template.BusinessLogic.DTOs;

namespace NetArch.Template.BusinessLogic.Interfaces.Commands;

public interface IProductCommands
{
    Task<int> CreateAsync(CreateProductRequest request, CancellationToken cancellationToken = default);
}
