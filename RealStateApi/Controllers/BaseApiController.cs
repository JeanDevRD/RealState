using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace RealStateApi.Controllers
{
    [Route("api/v{version:apiversion}/[controller]")]
    [ApiController]
    public class BaseApiController : ControllerBase
    {
        private IMediator _mediator;

        protected IMediator Mediator => _mediator ??= HttpContext.RequestServices.GetRequiredService<IMediator>();
    }
}
