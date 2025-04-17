using LoanApp.Models;
using System.ComponentModel.DataAnnotations;

namespace LoanApp.Features.DTOS
{
        public class LoanApplicationDraftDto
        {
            [Required(ErrorMessage = "Loan amount is required")]
            public double LoanAmount { get; set; }

            [Required(ErrorMessage = "Annual income is required")]
            public double AnnualIncome { get; set; }

            [Required(ErrorMessage = "Employment status is required")]
            public string EmploymentStatus { get; set; }

            [Required(ErrorMessage = "Credit score is required")]
            public int CreditScore { get; set; }

            [Required(ErrorMessage = "Residence type is required")]
            public string ResidenceType { get; set; }

            [Required(ErrorMessage = "Loan term is required")]
            public int LoanTerm { get; set; }

            [Required(ErrorMessage = "Property address is required")]
            public string PropertyAddress { get; set; }

            [Required(ErrorMessage = "Property value is required")]
            public double PropertyValue { get; set; }
            [Required(ErrorMessage = "Monthly Debts is required")]

            public double MonthlyDebts { get; set; }


    }
}


