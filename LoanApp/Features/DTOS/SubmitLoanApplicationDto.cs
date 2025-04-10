namespace LoanApp.Features.DTOS
{
    public record SubmitLoanApplicationDto(int? Id,double? LoanAmount, double? AnnualIncome, string? EmploymentStatus, int? CreditScore, string? ResidenceType, int? LoanTerm, string? PropertyAddress, double? PropertyValue,int UserId);
   
}
