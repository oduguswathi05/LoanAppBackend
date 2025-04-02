using LoanApp.Features.DTOS;
using LoanApp.Models;
using MediatR;

namespace LoanApp.Features.Queries.Get
{
    public record GetUserQuery(int id):IRequest<User>;
    
}
