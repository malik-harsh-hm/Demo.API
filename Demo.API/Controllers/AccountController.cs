using Asp.Versioning;
using Demo.API.Models;
using Demo.API.Repository;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Demo.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    //[ApiVersion("1.0")]

    public class AccountController : ControllerBase
    {
        public IAccountRepository _accountRepository { get; set; }
        public AccountController(IAccountRepository accountRepository)
        {
            this._accountRepository = accountRepository;
        }

        [HttpPost]
        [Route("SignUp")]
        [ProducesResponseType<IdentityResult>(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> SignUp([FromBody] SignUpModel data)
        {
            var result = await _accountRepository.SignUpAsync(data);
            if (result.Succeeded)
            {
                return Ok(result);
            }
            return Unauthorized();
        }
        [HttpPost]
        [Route("SignIn")]
        [ProducesResponseType<IdentityResult>(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> SignIn([FromBody] SignInModel data)
        {
            var result = await _accountRepository.SignInAsync(data);
            if (result != null)
            {
                return Ok(result);
            }
            return Unauthorized();
        }

    }
}
