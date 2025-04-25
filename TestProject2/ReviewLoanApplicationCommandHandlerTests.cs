using FluentAssertions;
using LoanApp.Data;
using LoanApp.Features.Commands.Update.LoanApplications;
using LoanApp.Features.DTOS;
using LoanApp.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestProject2
{
    public class ReviewLoanApplicationCommandHandlerTests
    {
        private readonly ApplicationDbContext _context;
        private readonly ReviewLoanApplicationCommandHandler _handler;

        public ReviewLoanApplicationCommandHandlerTests()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _context = new ApplicationDbContext(options);
            _handler = new ReviewLoanApplicationCommandHandler(_context);
        }

        [Fact]
        public async Task ShouldUpdateLoanStatusWhenValidReview()
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

            var dto = new ReviewApplicationDto
            (
                LoanStatus : "Approved",
                ReviewComment : "Looks good"
            );

            var command = new ReviewLoanApplicationCommand(application.Id, dto);

            await _handler.Handle(command, CancellationToken.None);

            var updatedApp = await _context.LoanApplications.FindAsync(application.Id);
            updatedApp.LoanStatus.Should().Be("Approved");
            updatedApp.ReviewComment.Should().Be("Looks good");
            updatedApp.ReviewedDate.Should().NotBeNull();
        }

        [Fact]
        public async Task ShouldThrowExceptionIfApplicationNotFound()
        {
            var dto = new ReviewApplicationDto
            (
                LoanStatus : "Rejected",
                ReviewComment : "Missing documents"
            );

            var command = new ReviewLoanApplicationCommand(999, dto);

            var act = async () => await _handler.Handle(command, CancellationToken.None);

            await act.Should().ThrowAsync<Exception>().WithMessage("Applicaton not found");
        }

        [Fact]
        public async Task ShouldThrowExceptionIfApplicationAlreadyReviewed()
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
                UserId = 2,
                LoanStatus = "Approved"
            };

            _context.LoanApplications.Add(application);
            await _context.SaveChangesAsync();

            var dto = new ReviewApplicationDto
            (
                LoanStatus : "Rejected",
                ReviewComment : "Sample"
            );

            var command = new ReviewLoanApplicationCommand(application.Id, dto);

            var act = async () => await _handler.Handle(command, CancellationToken.None);

            await act.Should().ThrowAsync<Exception>().WithMessage("Application already reviewed.");
        }
    }
}
