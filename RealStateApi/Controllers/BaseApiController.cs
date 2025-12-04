using Microsoft.AspNetCore.Mvc;

namespace RealStateApi.Controllers
{
    [Route("api/v{version:apiversion}/[controller]")]
    [ApiController]
    public class BaseApiController : ControllerBase
    {
        
    }
}
