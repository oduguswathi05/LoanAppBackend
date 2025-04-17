namespace LoanApp.Features.DTOS
{
    public class LoanApplicationsDto
    {     
            public int Id { get; set; }
            public double LoanAmount { get; set; }
            public double AnnualIncome { get; set; }
            public double PropertyValue { get; set; }
            public double MonthlyDebts { get; set; }
            public string EmploymentStatus { get; set; }
            public int CreditScore { get; set; }
            public string ResidenceType { get; set; }
            public int LoanTerm { get; set; }
            public string LoanStatus { get; set; }
            public DateTime ApplicationDate { get; set; }
            public string PropertyAddress { get; set; }
            public string? ReviewComment { get; set; }
            public DateTime? ReviewedDate { get; set; }
            public int UserId { get; set; }

            public double DTI => AnnualIncome > 0 ? (MonthlyDebts / (AnnualIncome / 12)) * 100 : 0;
            public double LTV => PropertyValue > 0 ? (LoanAmount / PropertyValue) * 100 : 0;
        

    }
}
