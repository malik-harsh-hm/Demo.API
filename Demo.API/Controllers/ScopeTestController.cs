using Asp.Versioning;
using Demo.API.Services;
using Microsoft.AspNetCore.Mvc;

namespace Demo.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    //[ApiVersion("1.0")]

    public class ScopeTestController : Controller
    {
        private readonly IFatherService _fatherService;
        private readonly IMotherService _motherService;
        public ScopeTestController(IFatherService fatherService, IMotherService motherService)
        {
            _fatherService = fatherService;
            _motherService = motherService;
        }

        [HttpGet]
        public IActionResult Get()
        {
            return Ok($"Father Creation Count : {FatherService.InstanceCount}. " +
                $"Mother Creation Count : {MotherService.InstanceCount}. " +
                $"Child Creation Count : {ChildService.InstanceCount}");
        }
    }
}
