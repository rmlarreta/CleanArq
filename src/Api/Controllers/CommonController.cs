using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [ApiController]
    [Route("api/v1/[Controller]/[Action]")]
    public abstract class CommonController : ControllerBase
    {
    }
}
