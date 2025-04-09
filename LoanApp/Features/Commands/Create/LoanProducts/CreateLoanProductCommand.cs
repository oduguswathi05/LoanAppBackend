using LoanApp.Features.DTOS;
using LoanApp.Models;
using MediatR;

namespace LoanApp.Features.Commands.Create.LoanProducts
{
    public record CreateLoanProductCommand(LoanProductDto LoanProductDto) : IRequest<int>;
  
}
