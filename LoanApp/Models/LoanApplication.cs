using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LoanApp.Models
{
    public class LoanApplication
    {
        public int Id { get; set; }
        public double LoanAmount { get; set; }
        public double AnnualIncome { get; set; }
        public string EmploymentStatus { get; set; }
        public int CreditScore { get; set; }
        public string ResidenceType { get; set; }
        public int LoanTerm { get; set; }
        public string LoanStatus { get; set; } = "Pending";
        public DateTime ApplicationDate { get; set; } = DateTime.Now;
        public double InterestRate { get; set; }
        public string PropertyAddress { get; set; }
        public double PropertyValue { get; set; }
        [ForeignKey("User")] 
        public int UserId { get; set; }

        public string? ReviewComment { get; set; }         
        public DateTime? ReviewedDate { get; set; }
    }

   

}



