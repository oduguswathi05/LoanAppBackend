using LoanApp.Features.DTOS;
using MediatR;

namespace LoanApp.Features.Commands.Create.Users
{
    public record CreateLoanOfficerCommand(UserRegisterDto loanOfficer) : IRequest<int?>;
   
}
