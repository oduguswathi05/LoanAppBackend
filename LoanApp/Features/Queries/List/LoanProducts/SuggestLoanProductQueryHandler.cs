using LoanApp.Data;
using LoanApp.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using static System.Net.Mime.MediaTypeNames;

namespace LoanApp.Features.Queries.List.LoanProducts
{
    public class SuggestLoanProductQueryHandler : IRequestHandler<SuggestLoanProductQuery, List<LoanProduct>>
    {
        private readonly ApplicationDbContext _context;

        public SuggestLoanProductQueryHandler(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<List<LoanProduct>> Handle(SuggestLoanProductQuery request, CancellationToken cancellationToken)
        {
            var application = request.Application;
            if(application.LoanStatus != "Approved")
            {
                throw new Exception("Your application either rejected or Pending");
            }
            return await _context.LoanProducts.Where(p =>p.IsActive&&application.LoanAmount >= p.MinLoanAmount &&application.LoanAmount <= p.MaxLoanAmount &&application.LoanTerm >= p.MinLoanTerm &&application.LoanTerm <= p.MaxLoanTerm && application.CreditScore >= p.MinCreditScore &&application.AnnualIncome >= p.MinAnnualIncome).ToListAsync();
        }
    }
}
