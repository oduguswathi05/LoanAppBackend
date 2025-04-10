using LoanApp.Data;
using LoanApp.Models;
using MediatR;

namespace LoanApp.Features.Queries.Get.LoanApplications
{
    public class GetLoanApplicationsByIdQueryHandler : IRequestHandler<GetLoanApplicationsByIdQuery, LoanApplication>
    {
        private readonly ApplicationDbContext _context;

        public GetLoanApplicationsByIdQueryHandler(ApplicationDbContext context)
        {
            _context = context;   
        }
        public async Task<LoanApplication> Handle(GetLoanApplicationsByIdQuery request, CancellationToken cancellationToken)
        {
            var loanApplication = await _context.LoanApplications.FindAsync(request.id, cancellationToken);
            if (loanApplication == null)
            {
                return null; 
            }
            return loanApplication;
        }
    }
}
