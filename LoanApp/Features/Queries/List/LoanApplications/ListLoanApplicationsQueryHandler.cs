using LoanApp.Data;
using LoanApp.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace LoanApp.Features.Queries.List.LoanApplications
{
    public class ListLoanApplicationsQueryHandler : IRequestHandler<ListLoanApplicationsQuery, List<LoanApplication>>
    {
        private readonly ApplicationDbContext _context;

        public ListLoanApplicationsQueryHandler(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<List<LoanApplication>> Handle(ListLoanApplicationsQuery request, CancellationToken cancellationToken)
        {
            var loanApplications = await _context.LoanApplications.Where(x => x.LoanStatus != "Draft").ToListAsync(cancellationToken);
            return loanApplications;
        }
    }
}
