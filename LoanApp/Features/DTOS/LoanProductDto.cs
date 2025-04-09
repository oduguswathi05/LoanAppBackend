using LoanApp.Models;
using System.ComponentModel.DataAnnotations;

namespace LoanApp.Features.DTOS
{
    public class LoanProductDto
    {
        [Required(ErrorMessage = "Product Name is required")]
        public string ProductName { get; set; }

        [Required(ErrorMessage = "Interest Rate is required")]
        public double InterestRate { get; set; }

        [Required(ErrorMessage = "Minimum Loan Amount is required")]
        public double MinLoanAmount { get; set; }

        [Required(ErrorMessage = "Maximum Loan Amount is required")]
        public double MaxLoanAmount { get; set; }

        [Required(ErrorMessage = "Minimum Loan Term is required")]
        public int MinLoanTerm { get; set; }

        [Required(ErrorMessage = "Maximum Loan Term is required")]
        public int MaxLoanTerm { get; set; }

    }
}
