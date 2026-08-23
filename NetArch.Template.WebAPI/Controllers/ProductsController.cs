#if (IsDapper || IsHybrid)
using Microsoft.AspNetCore.Mvc;
#if (IsClean)
using NetArch.Template.Application.DTOs;
using NetArch.Template.Application.Interfaces.Commands;
using NetArch.Template.Application.Interfaces.Queries;
#endif
#if (IsNTier)
using NetArch.Template.BusinessLogic.DTOs;
using NetArch.Template.BusinessLogic.Interfaces.Commands;
using NetArch.Template.BusinessLogic.Interfaces.Queries;
#endif

namespace NetArch.Template.WebAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ProductsController : BaseApiController
{
    private readonly IProductQueries _queries;
    private readonly IProductCommands _commands;

    public ProductsController(IProductQueries queries, IProductCommands commands)
    {
        _queries = queries;
        _commands = commands;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ProductSummaryDto>>> GetActive(CancellationToken cancellationToken)
        => Ok(await _queries.GetActiveAsync(cancellationToken));

    [HttpGet("{id:int}")]
    public async Task<ActionResult<ProductSummaryDto>> GetById(int id, CancellationToken cancellationToken)
    {
        var product = await _queries.GetByIdAsync(id, cancellationToken);
        return product is null ? NotFound() : Ok(product);
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateProductRequest request, CancellationToken cancellationToken)
    {
        var id = await _commands.CreateAsync(request, cancellationToken);
        return CreatedAtAction(nameof(GetById), new { id }, new { id });
    }
}
#endif
