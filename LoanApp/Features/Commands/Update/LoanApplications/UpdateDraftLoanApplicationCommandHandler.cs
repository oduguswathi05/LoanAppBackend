using LoanApp.Data;
using LoanApp.Models;
using MediatR;

namespace LoanApp.Features.Commands.Update.LoanApplication
{
    public class UpdateDraftLoanApplicationCommandHandler : IRequestHandler<UpdateDraftLoanApplicationCommand>
    {
        private readonly ApplicationDbContext _context;

        public UpdateDraftLoanApplicationCommandHandler(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task Handle(UpdateDraftLoanApplicationCommand request, CancellationToken cancellationToken)
        {
            var upLoanApplication = request.loanApplication;
            var existLoanApplication = await _context.LoanApplications.FindAsync(request.id);
            if (existLoanApplication == null)
                throw new InvalidOperationException("Loan draft not found.");
            if (existLoanApplication.LoanStatus != "Draft")
                throw new InvalidOperationException("Loan already submitted.");

            if (upLoanApplication.EmploymentStatus != null && upLoanApplication.EmploymentStatus != "")
            {
                existLoanApplication.EmploymentStatus = upLoanApplication.EmploymentStatus;
            }

            if (upLoanApplication.ResidenceType != null && upLoanApplication.ResidenceType != "")
            {
                existLoanApplication.ResidenceType = upLoanApplication.ResidenceType;
            }

            if (upLoanApplication.PropertyAddress != null && upLoanApplication.PropertyAddress != "")
            {
                existLoanApplication.PropertyAddress = upLoanApplication.PropertyAddress;
            }

            if (upLoanApplication.LoanAmount != null)
            {
                existLoanApplication.LoanAmount = upLoanApplication.LoanAmount.Value;
            }

            if (upLoanApplication.AnnualIncome != null)
            {
                existLoanApplication.AnnualIncome = upLoanApplication.AnnualIncome.Value;
            }

            if (upLoanApplication.CreditScore != null)
            {
                existLoanApplication.CreditScore = upLoanApplication.CreditScore.Value;
            }

            if (upLoanApplication.LoanTerm != null)
            {
                existLoanApplication.LoanTerm = upLoanApplication.LoanTerm.Value;
            }
            if (upLoanApplication.PropertyValue != null)
            {
                existLoanApplication.PropertyValue = upLoanApplication.PropertyValue.Value;
            }

            await _context.SaveChangesAsync(cancellationToken);

        }
    }
}
