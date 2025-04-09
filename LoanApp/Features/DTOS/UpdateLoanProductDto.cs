namespace LoanApp.Features.DTOS
{
    public record UpdateLoanProductDto(string? ProductName, double? InterestRate, double? MinLoanAmount, double? MaxLoanAmount, int? MinLoanTerm, int? MaxLoanTerm);
   
}
