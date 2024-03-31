using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;

namespace Demo.API.Controllers.v1
{
    [ApiController]
    [Route("api/v1/[controller]")]
    //[ApiVersion("1.0")]
    public class VersionTestController : ControllerBase
    {
        [HttpGet]
        [Route("")]
        public IActionResult Index()
        {
            return Ok("v1.0 Index called");
        }
    }
}
