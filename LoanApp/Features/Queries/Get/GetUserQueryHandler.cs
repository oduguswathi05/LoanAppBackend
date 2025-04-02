using LoanApp.Features.DTOS;
using LoanApp.Models;
using MediatR;

namespace LoanApp.Features.Queries.Get
{
    public class GetUserQueryHandler : IRequestHandler<GetUserQuery,User>
    {
        public Task<User> Handle(GetUserQuery request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
