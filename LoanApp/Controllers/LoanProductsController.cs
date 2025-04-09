using LoanApp.Features.Commands.Create.LoanProducts;
using LoanApp.Features.Commands.Delete;
using LoanApp.Features.Commands.Update.LoanProducts;
using LoanApp.Features.DTOS;
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
            await _mediatR.Send(new UpdateLoanProductCommand(id,dto));
            return NoContent();
        }


    }
}
