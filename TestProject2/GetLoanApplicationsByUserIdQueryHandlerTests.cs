using FluentAssertions;
using LoanApp.Data;
using LoanApp.Features.Queries.Get.LoanApplications;
using LoanApp.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestProject2
{
    public class GetLoanApplicationsByUserIdQueryHandlerTests
    {
        private readonly DbContextOptions<ApplicationDbContext> _DbContextOptions;
        private readonly ApplicationDbContext _context;
        private readonly GetLoanApplicationsByUserIdQueryHandler _handler;

        public GetLoanApplicationsByUserIdQueryHandlerTests()
        {
            _DbContextOptions = new DbContextOptionsBuilder<ApplicationDbContext>().UseInMemoryDatabase(databaseName: "DbTest4").Options;
            _context = new ApplicationDbContext(_DbContextOptions);
            _handler = new GetLoanApplicationsByUserIdQueryHandler(_context);
        }

        [Fact]
        public async Task ShouldReturnListOfApplicationsOfParticularUserTest()
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
                                                    UserId = 1
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
                                                    UserId = 2
                                                }
                                            );

            await _context.SaveChangesAsync();

            var command = new GetLoanApplicationsByUserIdQuery(1);

            var result =await _handler.Handle(command,default);

            result.Should().NotBeNull();
            result.Should().BeOfType<List<LoanApplication>>().Which.Count.Should().Be(2);
            result.All(l => l.UserId == 1).Should().BeTrue();


        }
        [Fact]
        public async Task ShouldReturnEmptyListWhenNoApplicationsExist()
        {
            var query = new GetLoanApplicationsByUserIdQuery(99);

            var result = await _handler.Handle(query, CancellationToken.None);

            result.Should().NotBeNull();
            result.Should().BeEmpty();
        }
    }
}
