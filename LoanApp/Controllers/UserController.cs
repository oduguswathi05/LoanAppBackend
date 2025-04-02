using LoanApp.Features.Queries.List;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LoanApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IMediator _meadiatR;

        public UserController(IMediator mediatR)
        {
            _meadiatR = mediatR;
        }
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var users = await _meadiatR.Send(new ListUsersQuery());
            return Ok(users);
        }
    }
}
