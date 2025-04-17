using LoanApp.Data;
using LoanApp.Features.DTOS;
using LoanApp.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace LoanApp.Features.Queries.List.LoanApplications
{
    public class ListLoanApplicationsQueryHandler : IRequestHandler<ListLoanApplicationsQuery, List<LoanApplicationsDto>>
    {
        private readonly ApplicationDbContext _context;

        public ListLoanApplicationsQueryHandler(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<List<LoanApplicationsDto>> Handle(ListLoanApplicationsQuery request, CancellationToken cancellationToken)
        {
            var loanApplications = await _context.LoanApplications.Where(x => x.LoanStatus != "Draft").ToListAsync(cancellationToken);
            var result = loanApplications.Select(app => new LoanApplicationsDto
            {
                Id = app.Id,
                LoanAmount = app.LoanAmount,
                AnnualIncome = app.AnnualIncome,
                PropertyValue = app.PropertyValue,
                MonthlyDebts = app.MonthlyDebts,
                EmploymentStatus = app.EmploymentStatus,
                CreditScore = app.CreditScore,
                ResidenceType = app.ResidenceType,
                LoanTerm = app.LoanTerm,
                LoanStatus = app.LoanStatus,
                ApplicationDate = app.ApplicationDate,
                PropertyAddress = app.PropertyAddress,
                ReviewComment = app.ReviewComment,
                ReviewedDate = app.ReviewedDate,
                UserId = app.UserId
            }).ToList();



            return result;
        }
    }
}
