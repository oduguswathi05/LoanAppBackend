using FluentAssertions;
using LoanApp.Data;
using LoanApp.Features.Commands.Create.LoanProducts;
using LoanApp.Features.DTOS;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace TestProject2.LoanProductHandlerTests
{
    public class CreateLoanProductCommandHandlerTests
    {
        private readonly ApplicationDbContext _context;
        private readonly CreateLoanProductCommandHandler _handler;

        public CreateLoanProductCommandHandlerTests()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase("LoanProductTestDb")
            .Options;

            _context = new ApplicationDbContext(options);
            _handler = new CreateLoanProductCommandHandler(_context);
        }

        [Fact]
        public async Task Should_CreateLoanProductSuccessfully()
        {
            var dto = new LoanProductDto
            {
                ProductName = "Home Loan",
                InterestRate = 5.5,
                MinLoanAmount = 100000,
                MaxLoanAmount = 500000,
                MinLoanTerm = 5,
                MaxLoanTerm = 30,
                MinCreditScore = 650,
                MinAnnualIncome = 30000
            };

            var command = new CreateLoanProductCommand(dto);

            var result = await _handler.Handle(command, CancellationToken.None);

            result.Should().BeGreaterThan(0);

            var addedProduct = await _context.LoanProducts.FindAsync(result);
            addedProduct.Should().NotBeNull();
            addedProduct.ProductName.Should().Be("Home Loan");
        }

    }
}
