using LoanApp.Features.Commands.Create.Users;
using LoanApp.Features.DTOS;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LoanApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserAuthController : ControllerBase
    {
        private readonly IMediator _meadiatR;

        public UserAuthController(IMediator mediatR)
        {
            _meadiatR = mediatR;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(UserLoginDto login)
        {
            var result = await _meadiatR.Send(new LoginUserCommand(login.Email, login.PasswordHash));
            if (result == null)
            {
                return Unauthorized("Invalid email or password.");
            }
            return Ok(result);
        }
    }
}
