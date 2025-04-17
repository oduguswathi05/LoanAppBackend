using LoanApp.Features.DTOS;
using MediatR;

namespace LoanApp.Features.Commands.Create.LoanApplications
{
    public record SubmitLoanApplicationCommand(SubmitLoanApplicationDto dto,int UserId):IRequest<LoanApplicationResultDto>;
   
}
