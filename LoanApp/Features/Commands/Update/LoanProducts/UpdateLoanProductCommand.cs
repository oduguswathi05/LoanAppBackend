using LoanApp.Features.DTOS;
using MediatR;

namespace LoanApp.Features.Commands.Update.LoanProducts
{
    public record UpdateLoanProductCommand(int id,UpdateLoanProductDto dto):IRequest;
  
}
