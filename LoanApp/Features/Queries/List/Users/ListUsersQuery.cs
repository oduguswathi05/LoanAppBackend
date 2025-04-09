using LoanApp.Models;
using MediatR;

namespace LoanApp.Features.Queries.List.Users
{
    public record ListUsersQuery : IRequest<List<User>>;

}
