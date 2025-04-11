using LoanApp.Models;
using MediatR;

namespace LoanApp.Features.Queries.Get.Users
{
    public record GetUserByIdQuery(int id):IRequest<User>;
    
}
