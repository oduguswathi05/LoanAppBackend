using LoanApp.Features.DTOS;
using MediatR;

namespace LoanApp.Features.Commands.Update.LoanApplication
{
    public record UpdateDraftLoanApplicationCommand(int id, UpdateApplicationDto loanApplication) : IRequest;


}
