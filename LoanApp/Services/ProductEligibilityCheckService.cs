using LoanApp.Features.DTOS;
using LoanApp.Models;
using static System.Net.Mime.MediaTypeNames;

namespace LoanApp.Services
{
    public class ProductEligibilityCheckService
    {
        public ProductEligibilityDto Evaluate(LoanApplication application,LoanProductDto product)
        {
            var ltv = (application.LoanAmount / application.PropertyValue) * 100;
            var dti = (application.MonthlyDebts / (application.AnnualIncome/12)) * 100;
            var adjustedRate = product.InterestRate;

            if (ltv > 90) adjustedRate += 1.0;
            else if (ltv > 80) adjustedRate += 0.5;

            if (application.CreditScore >= 740) adjustedRate += 0.0;
            else if (application.CreditScore >= 670) adjustedRate += 0.25;
            else if (application.CreditScore >= 580) adjustedRate += 0.5;
            else adjustedRate += 1.0;

            if (dti > 43) adjustedRate += 0.5;
            else if (dti > 36) adjustedRate += 0.25;

            string eligibility = "✅ Eligible";
            if (application.CreditScore < product.MinCreditScore)
                eligibility = "❌ Credit score too low";
            else if (application.AnnualIncome < product.MinAnnualIncome)
                eligibility = "❌ Income too low";
            //else if (application.LoanAmount < product.MinLoanAmount || application.LoanAmount > product.MaxLoanAmount)
            //    eligibility = "❌ Loan amount not supported";

            return new ProductEligibilityDto
            {
                ProductName = product.ProductName,
                AdjustedInterestRate = adjustedRate,
                EligibilityMessage = eligibility
            };
        }
    }
}
