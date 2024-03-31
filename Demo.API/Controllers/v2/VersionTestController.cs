using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;

namespace Demo.API.Controllers
{
    [ApiController]
    [Route("api/v2/[controller]")]
    //[ApiVersion("2.0")]
    public class VersionTestController : ControllerBase
    {
        [HttpGet]
        [Route("")]
        public IActionResult Index()
        {
            return Ok("v2.0 Index called");
        }
    }
}
