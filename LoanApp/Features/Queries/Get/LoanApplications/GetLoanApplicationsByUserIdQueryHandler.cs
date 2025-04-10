using LoanApp.Data;
using LoanApp.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace LoanApp.Features.Queries.Get.LoanApplications
{
    public class GetLoanApplicationsByUserIdQueryHandler : IRequestHandler<GetLoanApplicationsByUserIdQuery, List<LoanApplication>>
    {
        private readonly ApplicationDbContext _context;

        public GetLoanApplicationsByUserIdQueryHandler(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<List<LoanApplication>> Handle(GetLoanApplicationsByUserIdQuery request, CancellationToken cancellationToken)
        {
            return await _context.LoanApplications.Where(l => l.UserId == request.userId).ToListAsync(cancellationToken);
        }
    }
}
