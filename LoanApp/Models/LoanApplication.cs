using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LoanApp.Models
{
    public class Loan
    {
        public int Id { get; set; }

        [Required]
        public double LoanAmount { get; set; }
        
        [Required]
        public double AnnualIncome { get; set; }
       
        [Required]
        public string EmploymentStatus { get; set; }
        [Required]
        public int CreditScore { get; set; }

        [Required]
        public string ResidenceType { get; set; }

        [Required]
        public int LoanTerm { get; set; }

        [Required]
        public string LoanStatus { get; set; }

        [Required]
        public DateTime ApplicationDate { get; set; }
        [Required]
        public double InterestRate { get; set; }
        [Required]
        public string PropertyAddress { get; set; }
        [Required]
        public double PropertyValue { get; set; }

        [Required]
        [ForeignKey("User")] 
        public int UserId { get; set; }
    }

    public enum LoanStatus
    {
        Pending,    
        Approved,   
        Rejected
    }

}



