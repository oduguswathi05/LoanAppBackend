using LoanApp.Data;
using LoanApp.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace LoanApp.Features.Queries.List
{
    public class ListUsersQueryHandler : IRequestHandler<ListUsersQuery, List<User>>
    {
        private readonly ApplicationDbContext _context;

        public ListUsersQueryHandler(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<List<User>> Handle(ListUsersQuery request, CancellationToken cancellationToken)
        {
            var users = await _context.Users.ToListAsync(cancellationToken);
            return users;
        }

       
    }
}
