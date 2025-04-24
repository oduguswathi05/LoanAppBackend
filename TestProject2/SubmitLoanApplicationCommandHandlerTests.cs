using FluentAssertions;
using LoanApp.Data;
using LoanApp.Features.Commands.Create.LoanApplications;
using LoanApp.Features.DTOS;
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
    public class SubmitLoanApplicationCommandHandlerTests
    {
        private readonly DbContextOptions<ApplicationDbContext> _dbContextOptions;
        private readonly ApplicationDbContext _context;
        private readonly SubmitLoanApplicationCommandHandler _handler;

        public SubmitLoanApplicationCommandHandlerTests()
        {
            _dbContextOptions = new DbContextOptionsBuilder<ApplicationDbContext>().UseInMemoryDatabase(databaseName: "DbTest3").Options;
            _context = new ApplicationDbContext(_dbContextOptions);
            _handler = new SubmitLoanApplicationCommandHandler(_context);
        }
        [Fact]
        public async Task UpdateLoanApplicationOnlySpecificFieldsWhenAppIdGiven()
        {
            var existingApplication = new LoanApplication
            {
                LoanAmount = 100000,
                AnnualIncome = 50000,
                EmploymentStatus = "Employed",
                CreditScore = 700,
                ResidenceType = "Owned",
                LoanTerm = 30,
                PropertyAddress = "123 abc St",
                PropertyValue = 150000,
                MonthlyDebts = 500,
                UserId = 4,
                LoanStatus = "Draft"
            };
            _context.LoanApplications.Add(existingApplication);
            await _context.SaveChangesAsync();

            var applicationId = existingApplication.Id;

            var updatedDto = new SubmitLoanApplicationDto(
                               Id: applicationId,
                               LoanAmount: 120000,
                               AnnualIncome: null,
                               EmploymentStatus: "Self-Employed",
                               CreditScore: null,
                               ResidenceType: null,
                               LoanTerm: null,
                               PropertyAddress: null,
                               PropertyValue: null,
                               MonthlyDebts: null,
                               UserId: 4
                           );

            var command = new SubmitLoanApplicationCommand(updatedDto, updatedDto.UserId);
            await _handler.Handle(command, CancellationToken.None);

            var updatedApplication = await _context.LoanApplications.FindAsync(applicationId);
            updatedApplication.EmploymentStatus.Should().Be("Self-Employed");
            updatedApplication.ResidenceType.Should().Be("Owned");
            updatedApplication.PropertyAddress.Should().Be("123 abc St");
            updatedApplication.LoanAmount.Should().Be(120000);
            updatedApplication.AnnualIncome.Should().Be(50000);
            updatedApplication.CreditScore.Should().Be(700);
            updatedApplication.LoanTerm.Should().Be(30);
            updatedApplication.PropertyValue.Should().Be(150000);
            updatedApplication.MonthlyDebts.Should().Be(500);

        }

        //[Fact]
        //public async Task ThrowExceptionWhenPendingLoanApplicationExists()
        //{

        //    _context.LoanApplications.Add(new LoanApplication
        //    {
        //        LoanAmount = 100000,
        //        AnnualIncome = 50000,
        //        EmploymentStatus = "Employed",
        //        CreditScore = 700,
        //        ResidenceType = "Owned",
        //        LoanTerm = 30,
        //        PropertyAddress = "123 abc St",
        //        PropertyValue = 150000,
        //        MonthlyDebts = 500,
        //        UserId = 1,
        //        LoanStatus = "Pending"
        //    });
        //    await _context.SaveChangesAsync();

        //    var dto = new SubmitLoanApplicationDto(null, 150000, 60000, "Employed", 720, "Owned", 20, "Address", 180000, 300, 1);
        //    var command = new SubmitLoanApplicationCommand(dto, 1);

        //    var act = () => _handler.Handle(command, CancellationToken.None);
        //    await act.Should().ThrowAsync<Exception>().WithMessage("You already have a pending loan application under review.");
        //}

        //[Fact]
        //public async Task ThrowExceptionWhenApprovedLoanApplicationExists()
        //{

        //    _context.LoanApplications.Add(new LoanApplication
        //    {
        //        LoanAmount = 100000,
        //        AnnualIncome = 50000,
        //        EmploymentStatus = "Employed",
        //        CreditScore = 700,
        //        ResidenceType = "Owned",
        //        LoanTerm = 30,
        //        PropertyAddress = "123 abc St",
        //        PropertyValue = 150000,
        //        MonthlyDebts = 500,
        //        UserId = 1,
        //        LoanStatus = "Approved"
        //    });
        //    await _context.SaveChangesAsync();

        //    var dto = new SubmitLoanApplicationDto(null, 150000, 60000, "Employed", 720, "Owned", 20, "Address", 180000, 300, 1);
        //    var command = new SubmitLoanApplicationCommand(dto, 1);

        //    var act = () => _handler.Handle(command, CancellationToken.None);
        //    await act.Should().ThrowAsync<Exception>().WithMessage("Your Application already accepted and You can submit only one application");
        //}
        //[Fact]
        //public async Task SubmitLoanApplicationOnlySpecificFieldsWhenNoAppIdGiven()
        //{
        //    var dto = new SubmitLoanApplicationDto(null, 100000, 60000, "Employed", 760, "Owned", 15, "456 xyz Ave", 200000, 300, 2);
        //    var command = new SubmitLoanApplicationCommand(dto, 2);

        //    var result = await _handler.Handle(command, CancellationToken.None);

        //    var newApp = await _context.LoanApplications.FindAsync(result.id);
        //    newApp.Should().NotBeNull();
        //    newApp.UserId.Should().Be(2);
        //    newApp.EmploymentStatus.Should().Be("Employed");
        //    newApp.CreditScore.Should().Be(760);
        //    newApp.LoanStatus.Should().Be("Approved");

        //}
        //[Fact]
        //public async Task ReturnPendingStatusWhenLoanCriteriaNotMet()
        //{ 

        //    var dto = new SubmitLoanApplicationDto(null, 200000, 40000, "Employed", 650, "Rented", 25, "789 def Blvd", 180000, 700, 3);
        //    var command = new SubmitLoanApplicationCommand(dto, 3);

        //    var result = await _handler.Handle(command, CancellationToken.None);

        //    var newApp = await _context.LoanApplications.FindAsync(result.id);
        //    newApp.Should().NotBeNull();
        //    newApp.UserId.Should().Be(3);
        //    newApp.LoanStatus.Should().Be("Pending");
        //}
    }
}
