namespace LoanApp.Features.DTOS
{
    public record SubmitLoanApplicationDto(int? LoanId,double? LoanAmount, double? AnnualIncome, string? EmploymentStatus, int? CreditScore, string? ResidenceType, int? LoanTerm, double? InterestRate, string? PropertyAddress, double? PropertyValue,int UserId);
   
}
