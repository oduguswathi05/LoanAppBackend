using FluentAssertions;
using LoanApp.Data;
using LoanApp.Features.Commands.Create.LoanApplications;
using LoanApp.Features.Queries.List.LoanApplications;
using LoanApp.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace TestProject2
{
    public class ListLoanApplicationsQueryHandlerTests: IAsyncLifetime
    {
        private readonly DbContextOptions<ApplicationDbContext> _dbContextOptions;
        private readonly ApplicationDbContext _context;
        private readonly ListLoanApplicationsQueryHandler _handler;

        public ListLoanApplicationsQueryHandlerTests()
        {
            _dbContextOptions = new DbContextOptionsBuilder<ApplicationDbContext>().UseInMemoryDatabase(databaseName: "DbTest").Options;
            _context = new ApplicationDbContext(_dbContextOptions);
            _handler = new ListLoanApplicationsQueryHandler(_context);
        }

        [Fact]
        public async Task ReturnOnlyNonDraftApplications()
        {
            _context.LoanApplications.AddRange(
                                                new LoanApplication
                                                {
                                                   
                                                    LoanStatus = "Approved",
                                                    LoanAmount = 100000,
                                                    AnnualIncome = 50000,
                                                    PropertyValue = 200000,
                                                    MonthlyDebts = 500,
                                                    EmploymentStatus = "Full-time",
                                                    CreditScore = 700,
                                                    ResidenceType = "Owned",
                                                    LoanTerm = 15,
                                                    PropertyAddress = "123 Main St",
                                                    ApplicationDate = DateTime.UtcNow,
                                                    UserId = 1
                                                },
                                                new LoanApplication
                                                {
                                                  
                                                    LoanStatus = "Rejected",
                                                    LoanAmount = 80000,
                                                    AnnualIncome = 40000,
                                                    PropertyValue = 150000,
                                                    MonthlyDebts = 300,
                                                    EmploymentStatus = "Self-employed",
                                                    CreditScore = 650,
                                                    ResidenceType = "Rented",
                                                    LoanTerm = 10,
                                                    PropertyAddress = "456 Elm St",
                                                    ApplicationDate = DateTime.UtcNow,
                                                    UserId = 2
                                                },
                                                new LoanApplication
                                                {
                                                   
                                                    LoanStatus = "Draft", 
                                                    LoanAmount = 200000,
                                                    AnnualIncome = 60000,
                                                    PropertyValue = 250000,
                                                    MonthlyDebts = 400,
                                                    EmploymentStatus = "Contract",
                                                    CreditScore = 720,
                                                    ResidenceType = "Owned",
                                                    LoanTerm = 20,
                                                    PropertyAddress = "789 Oak St",
                                                    ApplicationDate = DateTime.UtcNow,
                                                    UserId = 3
                                                }
                                            );

            await _context.SaveChangesAsync();

            var result = await _handler.Handle(new ListLoanApplicationsQuery(), CancellationToken.None);

            result.Should().HaveCount(2);
            result.Should().NotContain(r => r.LoanStatus == "Draft");
            result.Should().OnlyContain(r => r.LoanStatus == "Approved" || r.LoanStatus == "Rejected");
        }

        [Fact]
        public async Task NoApplicationsReturnsEmptyList()
        {
            var result = await _handler.Handle(new ListLoanApplicationsQuery(), CancellationToken.None);

            result.Should().BeEmpty();
        }

        public async Task InitializeAsync()
        {
            await _context.Database.EnsureDeletedAsync();
            await _context.Database.EnsureCreatedAsync();
        }

        public Task DisposeAsync()
        {
            _context.Dispose();
            return Task.CompletedTask;
        }

        

    }
}
