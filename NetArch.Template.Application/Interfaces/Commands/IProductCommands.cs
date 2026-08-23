using NetArch.Template.Application.DTOs;

namespace NetArch.Template.Application.Interfaces.Commands;

public interface IProductCommands
{
    Task<int> CreateAsync(CreateProductRequest request, CancellationToken cancellationToken = default);
}
