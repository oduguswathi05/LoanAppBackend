using LoanApp.Features.Commands.Create;
using LoanApp.Features.Commands.Create.LoanApplications;
using LoanApp.Features.Commands.Update;
using LoanApp.Features.Commands.Update.LoanApplication;
using LoanApp.Features.Commands.Update.LoanApplications;
using LoanApp.Features.DTOS;
using LoanApp.Features.Queries.Get.LoanApplications;
using LoanApp.Features.Queries.List.LoanApplications;
using LoanApp.Models;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace LoanApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoanApplicationsController : ControllerBase
    {
        private readonly IMediator _mediatR;

        public LoanApplicationsController(IMediator mediatR)
        {
            _mediatR = mediatR;
        }

        [HttpPost("Draft")]
        [Authorize(Roles = "Customer")]
        public async Task<IActionResult> Post(LoanApplicationDto loanApplication) {
            try { 
                var userIdClaim = User.FindFirst("userId");
                if (userIdClaim == null)
                {
                    return Unauthorized("User ID claim is missing.");
                }

                var userId = int.Parse(userIdClaim.Value);
                var LoanApplicationId = await _mediatR.Send(new CreateLoanApplicationCommand(loanApplication, userId));
                return Ok(LoanApplicationId);
            }
            catch (Exception ex)
            {
                return  BadRequest(ex.Message);
            }

        }

        [HttpGet]
        [Authorize(Roles = "LoanOfficer")]
        public async Task<IActionResult> Get()
        {
            var loanApplications = await _mediatR.Send(new ListLoanApplicationsQuery());
            return Ok(loanApplications);
        }

        [HttpPut("Draft/{id}")]
        [Authorize(Roles = "Customer")]
        public async Task<IActionResult> Update(int id,UpdateApplicationDto dto)
        {
           
            await _mediatR.Send(new UpdateDraftLoanApplicationCommand(id,dto));
            return NoContent();
        }

        [HttpPost("Submit")]
        [Authorize(Roles = "Customer")]
        public async Task<IActionResult> SubmitLoan(SubmitLoanApplicationDto dto)
        {
            try
            {
                var userIdClaim = User.FindFirst("userId");
                if (userIdClaim == null)
                {
                    return Unauthorized("User ID claim is missing.");
                }

                var userId = int.Parse(userIdClaim.Value);

                var loanId = await _mediatR.Send(new SubmitLoanApplicationCommand(dto, userId));

                return Ok(loanId);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("UserId")]
        [Authorize(Roles = "Customer")]
        public async Task<IActionResult> GetLoanApplicationsByUserId()
        {
            var userIdClaim = User.FindFirst("userId");
            if (userIdClaim == null)
            {
                return Unauthorized("User ID claim is missing.");
            }

            var userId = int.Parse(userIdClaim.Value);
            var loanApplications = await _mediatR.Send(new GetLoanApplicationsByUserIdQuery(userId));
            return Ok(loanApplications);
        }

        [HttpPut("Review/{id}")]
        [Authorize(Roles = "LoanOfficer")]
        public async Task<IActionResult> UpdateLoanStatus(int id,ReviewApplicationDto dto)
        {
            try
            {
                await _mediatR.Send(new ReviewLoanApplicationCommand(id, dto));
                return NoContent();
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
           
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "Customer")]
        public async Task<IActionResult> GetLoanApplicationsById(int id)
        {
            try
            {
                var loanApplication = await _mediatR.Send(new GetLoanApplicationsByIdQuery(id));
                if (loanApplication == null)
                {
                    return NotFound();
                }
                return Ok(loanApplication);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
           
        }

        


    }
}
