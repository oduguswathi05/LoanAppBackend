﻿using MediatR;

namespace LoanApp.Features.DTOS
{
    public record UpdateApplicationDto(double? LoanAmount, double? AnnualIncome, string? EmploymentStatus, int? CreditScore, string? ResidenceType, int? LoanTerm, string? PropertyAddress, double? PropertyValue, double? MonthlyDebts);
  
}
