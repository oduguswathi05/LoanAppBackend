using LoanApp.Features.Commands.Create.LoanProducts;
using LoanApp.Features.Commands.Delete;
using LoanApp.Features.Commands.Update.LoanProducts;
using LoanApp.Features.DTOS;
using LoanApp.Features.Queries.Get.LoanApplications;
using LoanApp.Features.Queries.List.LoanProducts;
using LoanApp.Models;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LoanApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoanProductsController : ControllerBase
    {
        private readonly IMediator _mediatR;

        public LoanProductsController(IMediator mediatR)
        {
            _mediatR = mediatR;
        }

        [HttpPost]
        [Authorize(Roles = "LoanOfficer")]
        public async Task<IActionResult> Add(LoanProductDto loanProduct)
        {
            var LoanProductId = await _mediatR.Send(new CreateLoanProductCommand(loanProduct));
            return Ok(LoanProductId);
        }

        [HttpGet]
        [Authorize(Roles = "LoanOfficer")]
        public async Task<IActionResult> Get()
        {
            var products = await _mediatR.Send(new ListLoanProductsQuery());
            return Ok(products);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "LoanOfficer")]
        public async Task<IActionResult> Delete(int id)
        {
            await _mediatR.Send(new DeleteLoanProductCommand(id));
            return NoContent();
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "LoanOfficer")]
        public async Task<IActionResult> Update(int id,UpdateLoanProductDto dto)
        {
            try
            {
                await _mediatR.Send(new UpdateLoanProductCommand(id, dto));
                return NoContent();
            }
            catch(Exception ex)
            {
                return StatusCode(500, new
                {
                    error = "Something went wrong",
                    details = ex.Message 
                });

            }

        }

        [HttpGet("Suggest")]
        [Authorize]
        public async Task<IActionResult> SuggestLoanProducts()
        {
            try
            {
                var userIdClaim = User.FindFirst("userId");
                if (userIdClaim == null)
                {
                    return Unauthorized("User ID claim is missing.");
                }

                var userId = int.Parse(userIdClaim.Value);
                var applications = await _mediatR.Send(new GetLoanApplicationsByUserIdQuery(userId));
                var approvedApplication = applications.FirstOrDefault(app => app.LoanStatus == "Approved");
                if (approvedApplication == null)
                {
                    return NotFound("No approved loan application found for this user.");
                }
                var products = await _mediatR.Send(new SuggestLoanProductQuery(approvedApplication));
                return Ok(products);
            }
             catch(Exception ex)
            {
                return BadRequest(ex);
            }
            
        }


    }
}
