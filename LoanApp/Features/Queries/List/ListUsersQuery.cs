using LoanApp.Models;
using MediatR;

namespace LoanApp.Features.Queries.List
{
    public record ListUsersQuery:IRequest<List<User>>;
    
}
