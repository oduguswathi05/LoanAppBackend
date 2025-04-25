using FluentAssertions;
using LoanApp.Data;
using LoanApp.Features.Queries.Get.LoanApplications;
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
    public class GetLoanApplicationsByIdQueryHandlerTests
    {
        private readonly DbContextOptions<ApplicationDbContext> _DbContextOptions;
        private readonly ApplicationDbContext _context;
        private readonly GetLoanApplicationsByIdQueryHandler _handler;

        public GetLoanApplicationsByIdQueryHandlerTests()
        {
            _DbContextOptions = new DbContextOptionsBuilder<ApplicationDbContext>().UseInMemoryDatabase(databaseName: "DbTest5").Options;
            _context = new ApplicationDbContext(_DbContextOptions);
            _handler = new GetLoanApplicationsByIdQueryHandler(_context);
        }
        [Fact]
        public async Task ShouldReturnLoanApplicationByIdTest()
        {
            var application = new LoanApplication
            {
                LoanAmount = 200000,
                AnnualIncome = 40000,
                EmploymentStatus = "Employed",
                CreditScore = 750,
                ResidenceType = "Owned",
                LoanTerm = 25,
                PropertyAddress = "123 abc St",
                PropertyValue = 300000,
                MonthlyDebts = 700,
                UserId = 1,
                LoanStatus = "Pending"
            };

            _context.LoanApplications.Add(application);
            await _context.SaveChangesAsync();

            var command = new GetLoanApplicationsByIdQuery(application.Id);

            var result = await _handler.Handle(command, default);

            result.Should().NotBeNull();
            result.Id.Should().Be(application.Id);
            result.LoanAmount.Should().Be(200000);
            result.AnnualIncome.Should().Be(40000);
            result.EmploymentStatus.Should().Be("Employed");
            result.CreditScore.Should().Be(750);
            result.ResidenceType.Should().Be("Owned");
            result.LoanTerm.Should().Be(25);
            result.PropertyAddress.Should().Be("123 abc St");
            result.PropertyValue.Should().Be(300000);
            result.MonthlyDebts.Should().Be(700);
            result.UserId.Should().Be(1);
            result.LoanStatus.Should().Be("Pending");

        }
        [Fact]
        public async Task ShouldReturnNullNoApplicationExist()
        {
            var query = new GetLoanApplicationsByIdQuery(99);

            var result = await _handler.Handle(query, CancellationToken.None);

            result.Should().BeNull();
        }
    }
    
}
