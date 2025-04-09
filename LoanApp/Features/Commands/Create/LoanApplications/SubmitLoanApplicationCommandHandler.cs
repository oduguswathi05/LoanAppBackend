using LoanApp.Data;
using LoanApp.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace LoanApp.Features.Commands.Create.LoanApplications
{
    public class SubmitLoanApplicationCommandHandler : IRequestHandler<SubmitLoanApplicationCommand, int>
    {
        private readonly ApplicationDbContext _context;

        public SubmitLoanApplicationCommandHandler(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<int> Handle(SubmitLoanApplicationCommand request, CancellationToken cancellationToken)
        {

            var upLoanApplication = request.dto;
            //var existingApplication = await _context.Loans.FirstOrDefaultAsync(loan => loan.UserId == request.UserId && loan.LoanStatus == "Pending", cancellationToken);

            //if (existingApplication != null)
            //{
            //    throw new Exception("You already have a pending loan application under review.");
            //}

            if (upLoanApplication.LoanId != null)
            {
                var existLoanApplication = await _context.Loans.FindAsync(upLoanApplication.LoanId);
               

                if (existLoanApplication == null)
                    throw new Exception("Loan draft not found.");

                if (existLoanApplication.LoanStatus != "Draft")
                    throw new Exception("Loan already submitted.");

                existLoanApplication.LoanStatus = "Pending";

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

                if (upLoanApplication.InterestRate != null)
                {
                    existLoanApplication.InterestRate = upLoanApplication.InterestRate.Value;
                }

                if (upLoanApplication.PropertyValue != null)
                {
                    existLoanApplication.PropertyValue = upLoanApplication.PropertyValue.Value;
                }

                await _context.SaveChangesAsync(cancellationToken);
                return existLoanApplication.Id;
            }
            else
            {
                var newLoanApplication = new LoanApplication
                {
                    LoanAmount = upLoanApplication.LoanAmount.Value,
                    AnnualIncome = upLoanApplication.AnnualIncome.Value,
                    EmploymentStatus = upLoanApplication.EmploymentStatus,
                    CreditScore = upLoanApplication.CreditScore.Value,
                    ResidenceType = upLoanApplication.ResidenceType,
                    LoanTerm = upLoanApplication.LoanTerm.Value,
                    InterestRate = upLoanApplication.InterestRate.Value,
                    PropertyAddress = upLoanApplication.PropertyAddress,
                    PropertyValue = upLoanApplication.PropertyValue.Value,
                    UserId = request.UserId

                };

                await _context.Loans.AddAsync(newLoanApplication, cancellationToken);
                await _context.SaveChangesAsync();
                return newLoanApplication.Id;
            }
        }
    }
}
