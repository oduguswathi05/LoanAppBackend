using LoanApp.Data;
using LoanApp.Features.DTOS;
using LoanApp.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace LoanApp.Features.Commands.Create.LoanApplications
{
    public class SubmitLoanApplicationCommandHandler : IRequestHandler<SubmitLoanApplicationCommand, LoanApplicationResultDto>
    {
        private readonly ApplicationDbContext _context;

        public SubmitLoanApplicationCommandHandler(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<LoanApplicationResultDto> Handle(SubmitLoanApplicationCommand request, CancellationToken cancellationToken)
        {

            var upLoanApplication = request.dto;
            
            var existingApplication = await _context.LoanApplications.FirstOrDefaultAsync(loan => loan.UserId == request.UserId && loan.LoanStatus == "Pending", cancellationToken);

            if (existingApplication != null)
            {
                throw new Exception("You already have a pending loan application under review.");
            }

            var existingApplication1 = await _context.LoanApplications.FirstOrDefaultAsync(loan => loan.UserId == request.UserId && loan.LoanStatus == "Approved", cancellationToken);
            if (existingApplication1 != null)
            {
                throw new Exception("Your Application already accepted and You can submit only one application");
            }

           

            if (upLoanApplication.Id != null)
            {

                var existLoanApplication = await _context.LoanApplications.FindAsync(upLoanApplication.Id);
                if (existLoanApplication == null)
                    throw new Exception("Loan draft not found.");

                if (existLoanApplication.LoanStatus != "Draft")
                    throw new Exception("Loan already submitted.");

                double monthlyIncome = existLoanApplication.AnnualIncome / 12;
                double dti = monthlyIncome > 0 ? (existLoanApplication.MonthlyDebts / monthlyIncome) * 100 : 0;
                double ltv = existLoanApplication.PropertyValue > 0 ? (existLoanApplication.LoanAmount / existLoanApplication.PropertyValue) * 100 : 0;

                if (existLoanApplication.CreditScore >= 750 && dti <= 36 && ltv <= 80)
                {
                    existLoanApplication.LoanStatus = "Approved";
                    existLoanApplication.ReviewedDate = DateTime.Now;
                }
                else
                {
                    existLoanApplication.LoanStatus = "Pending";
                }


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
                if (upLoanApplication.MonthlyDebts != null)
                {
                    existLoanApplication.MonthlyDebts = upLoanApplication.MonthlyDebts.Value;
                }

                await _context.SaveChangesAsync(cancellationToken);
                return new LoanApplicationResultDto(existingApplication.Id, existingApplication.LoanStatus);
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
                    PropertyAddress = upLoanApplication.PropertyAddress,
                    PropertyValue = upLoanApplication.PropertyValue.Value,
                    MonthlyDebts = upLoanApplication.MonthlyDebts.Value,
                    UserId = request.UserId
                };
                double monthlyIncome = newLoanApplication.AnnualIncome / 12;
                double dti = monthlyIncome > 0 ? (newLoanApplication.MonthlyDebts / monthlyIncome) * 100 : 0;
                double ltv = newLoanApplication.PropertyValue > 0 ? (newLoanApplication.LoanAmount / newLoanApplication.PropertyValue) * 100 : 0;

                if (newLoanApplication.CreditScore >= 750 && dti <= 36 && ltv <= 80)
                {
                    newLoanApplication.LoanStatus = "Approved";
                    newLoanApplication.ReviewedDate = DateTime.Now;
                }
                else
                {
                    newLoanApplication.LoanStatus = "Pending";
                }


                await _context.LoanApplications.AddAsync(newLoanApplication, cancellationToken);
                await _context.SaveChangesAsync();
                return new LoanApplicationResultDto(newLoanApplication.Id, newLoanApplication.LoanStatus);

            }


        }
    }
}
