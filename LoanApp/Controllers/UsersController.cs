using LoanApp.Features.Commands.Create.Users;
using LoanApp.Features.DTOS;
using LoanApp.Features.Queries.Get.Users;
using LoanApp.Features.Queries.List.Users;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LoanApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IMediator _meadiatR;

        public UsersController(IMediator mediatR)
        {
            _meadiatR = mediatR;
        }
        [HttpGet]
        [Authorize(Roles = "LoanOfficer")]
        public async Task<IActionResult> Get()
        {
            var users = await _meadiatR.Send(new ListUsersQuery());
            return Ok(users);
        }

        [HttpPost("register/customer")]
        public async Task<IActionResult> RegisterC(UserRegisterDto user)
        {
            var UserId = await _meadiatR.Send(new CreateCustomerCommand(user));
            if (UserId == null)
                return BadRequest("Email is already in use.");
            return Ok(UserId);
        }

        [HttpPost("register/loanOfficer")]
        public async Task<IActionResult> RegisterO(UserRegisterDto user)
        {
            var UserId = await _meadiatR.Send(new CreateLoanOfficerCommand(user));
            if (UserId == null)
                return BadRequest("Email is already in use.");
            return Ok(UserId);
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "LoanOfficer")]
        public async Task<IActionResult> Get(int id)
        {
            var user = await _meadiatR.Send(new GetUserByIdQuery(id));
            if(user == null)
            {
                return NotFound();
            }
            return Ok(user);
        }
    }
}
