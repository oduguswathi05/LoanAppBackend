using LoanApp.Data;
using LoanApp.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace LoanApp.Features.Commands.Create.LoanApplications
{
    public class CreateDraftLoanApplicationCommandHandler : IRequestHandler<CreateDraftLoanApplicationCommand, int>
    {
        private readonly ApplicationDbContext _context;

        public CreateDraftLoanApplicationCommandHandler(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<int> Handle(CreateDraftLoanApplicationCommand request, CancellationToken cancellationToken)
        {
           
            var loanApplication = request.LoanApplicationDto;
            var newLoanApplication = new LoanApplication
            {
                LoanAmount = loanApplication.LoanAmount,
                AnnualIncome = loanApplication.AnnualIncome,
                EmploymentStatus = loanApplication.EmploymentStatus,
                CreditScore = loanApplication.CreditScore,
                ResidenceType = loanApplication.ResidenceType,
                LoanTerm = loanApplication.LoanTerm,
                LoanStatus = "Draft",
                PropertyAddress = loanApplication.PropertyAddress,
                PropertyValue = loanApplication.PropertyValue,
                MonthlyDebts = loanApplication.MonthlyDebts,
                UserId = request.userId

            };

            await _context.LoanApplications.AddAsync(newLoanApplication, cancellationToken);
            await _context.SaveChangesAsync();
            return newLoanApplication.Id;
        }
    }
}
