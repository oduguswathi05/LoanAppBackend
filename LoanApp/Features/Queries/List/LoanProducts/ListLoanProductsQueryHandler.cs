using LoanApp.Data;
using LoanApp.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace LoanApp.Features.Queries.List.LoanProducts
{
    public class ListLoanProductsQueryHandler : IRequestHandler<ListLoanProductsQuery, List<LoanProduct>>
    {
        private readonly ApplicationDbContext _context;

        public ListLoanProductsQueryHandler(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<List<LoanProduct>> Handle(ListLoanProductsQuery request, CancellationToken cancellationToken)
        {
            var products = await _context.LoanProducts.Where(p=>p.IsActive == true).ToListAsync(cancellationToken);
            return products;
        }
    }
}
