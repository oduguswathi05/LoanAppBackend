using MediatR;

namespace LoanApp.Features.Commands.Create
{
    public record CreateUserCommand(string FirstName,string LastName, string Email,string PasswordHash, string ConfirmPassword,string PhoneNumber,string Role):IRequest<int>;
   
}
