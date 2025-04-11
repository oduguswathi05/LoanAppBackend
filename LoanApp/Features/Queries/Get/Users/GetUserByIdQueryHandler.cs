using LoanApp.Data;
using LoanApp.Models;
using MediatR;

namespace LoanApp.Features.Queries.Get.Users
{
    public class GetUserByIdQueryHandler : IRequestHandler<GetUserByIdQuery, User>
    {
        private readonly ApplicationDbContext _context;

        public GetUserByIdQueryHandler(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<User> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
        {
            var user = await _context.Users.FindAsync(request.id,cancellationToken);
            if (user == null)
            {
                return null;
            }
            return user;
        }
    }
}
