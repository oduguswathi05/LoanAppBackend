using LoanApp.Data;
using LoanApp.Models;
using MediatR;

namespace LoanApp.Features.Commands.Create.LoanApplications
{
    public class CreateLoanApplicationCommandHandler : IRequestHandler<CreateLoanApplicationCommand, int>
    {
        private readonly ApplicationDbContext _context;

        public CreateLoanApplicationCommandHandler(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<int> Handle(CreateLoanApplicationCommand request, CancellationToken cancellationToken)
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
                InterestRate = loanApplication.InterestRate,
                PropertyAddress = loanApplication.PropertyAddress,
                PropertyValue = loanApplication.PropertyValue,
                UserId = request.userId

            };

            await _context.Loans.AddAsync(newLoanApplication, cancellationToken);
            await _context.SaveChangesAsync();
            return newLoanApplication.Id;
        }
    }
}
