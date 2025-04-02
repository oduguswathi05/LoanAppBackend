using MediatR;

namespace LoanApp.Features.DTOS
{
    public record UserLoginDto(string Email, string PasswordHash);

}
