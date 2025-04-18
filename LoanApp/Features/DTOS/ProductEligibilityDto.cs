namespace LoanApp.Features.DTOS
{
    public class ProductEligibilityDto
    {
        public string ProductName { get; set; }
        public double AdjustedInterestRate { get; set; }
        public string EligibilityMessage { get; set; }
    }
}
