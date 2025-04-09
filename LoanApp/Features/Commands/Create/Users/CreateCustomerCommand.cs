using LoanApp.Features.DTOS;
using MediatR;

namespace LoanApp.Features.Commands.Create.Users
{
    public record CreateCustomerCommand(UserRegisterDto customer) : IRequest<int?>;

}
