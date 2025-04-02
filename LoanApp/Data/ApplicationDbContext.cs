using LoanApp.Models;
using Microsoft.EntityFrameworkCore;

namespace LoanApp.Data
{
    public class ApplicationDbContext:DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options):base(options) 
        {
            
        }

        public DbSet<User> Users { get; set; }
        public DbSet<LoanApplication> Loans { get; set; }
        public DbSet<LoanProduct> LoanProducts { get; set; }

    }
}
