using Microsoft.EntityFrameworkCore;

namespace LoanApp.Models
{
    public class LoanProduct
    {
        public int Id { get; set; }
        public string ProductName { get; set; }
        public double InterestRate { get; set; }
        public double MinLoanAmount { get; set; }
        public double MaxLoanAmount { get; set; }
        public int MinLoanTerm { get; set; } 
        public int MaxLoanTerm { get; set; }
        public string EligibilityCriteria { get; set; }
        public bool IsActive { get; set; } = true; 
        public LoanType LoanType { get; set; } 
    }

    public enum LoanType
    {
        FixedRateMortgage,
        AdjustableRateMortgage,
        FHA,   
        VA,    
        USDA, 
        JumboLoan
    }

}
