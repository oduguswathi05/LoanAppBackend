using LoanApp.Features.DTOS;
using LoanApp.Models;
using MediatR;

namespace LoanApp.Features.Commands.Create.LoanApplications
{
    public record CreateLoanApplicationCommand(LoanApplicationDto LoanApplicationDto,int userId) : IRequest<int>;

}
