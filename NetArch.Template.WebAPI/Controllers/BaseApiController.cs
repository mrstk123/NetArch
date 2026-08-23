using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace NetArch.Template.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public abstract class BaseApiController : ControllerBase
    {
    }
}
