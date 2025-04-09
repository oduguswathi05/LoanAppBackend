using LoanApp.Features.DTOS;
using MediatR;

namespace LoanApp.Features.Commands.Create.Users
{
    public record LoginUserCommand(string Email,string Password):IRequest<LoginResponseDto>;
   
}
