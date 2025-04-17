using LoanApp.Features.DTOS;
using LoanApp.Models;
using MediatR;

namespace LoanApp.Features.Commands.Create.LoanApplications
{
    public record CreateDraftLoanApplicationCommand(LoanApplicationDraftDto LoanApplicationDto,int userId) : IRequest<int>;

}
