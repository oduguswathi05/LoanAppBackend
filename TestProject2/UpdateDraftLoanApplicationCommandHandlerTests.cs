using FluentAssertions;
using LoanApp.Data;
using LoanApp.Features.Commands.Update.LoanApplication;
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
    public class UpdateDraftLoanApplicationCommandHandlerTests
    {
        private readonly DbContextOptions<ApplicationDbContext> _dbContextOptions;
        private readonly ApplicationDbContext _context;
        private readonly UpdateDraftLoanApplicationCommandHandler _handler;

        public UpdateDraftLoanApplicationCommandHandlerTests()
        {
            _dbContextOptions = new DbContextOptionsBuilder<ApplicationDbContext>().UseInMemoryDatabase(databaseName: "DbTest1").Options;
            _context = new ApplicationDbContext(_dbContextOptions);
            _handler = new UpdateDraftLoanApplicationCommandHandler(_context);
        }
        [Fact]
        public async Task UpdateDraftLoanApplicationTest()
        {
            var draftApplication = new LoanApplication
            {
                LoanStatus = "Draft",
                EmploymentStatus = "Part-time",
                ResidenceType = "Rented",
                PropertyAddress = "456 abc St",
                LoanAmount = 50000,
                AnnualIncome = 40000,
                CreditScore = 650,
                LoanTerm = 10,
                PropertyValue = 150000,
                MonthlyDebts = 300,
                ApplicationDate = DateTime.UtcNow,
                UserId = 2
            };
            _context.LoanApplications.Add(draftApplication);
            await _context.SaveChangesAsync();

            var updatedDto = new UpdateApplicationDto
                                (
                                    LoanAmount: 100000,
                                    AnnualIncome: 50000,
                                    EmploymentStatus: "Full-time",
                                    CreditScore: 700,
                                    ResidenceType: "Owned",
                                    LoanTerm: 15,
                                    PropertyAddress: "123 Main St",
                                    PropertyValue: 200000,
                                    MonthlyDebts: 500
                                );

            var command = new UpdateDraftLoanApplicationCommand(draftApplication.Id, updatedDto);
           
            await _handler.Handle(command, CancellationToken.None);

            var updatedApplication = await _context.LoanApplications.FindAsync(draftApplication.Id);
            updatedApplication.EmploymentStatus.Should().Be("Full-time");
            updatedApplication.ResidenceType.Should().Be("Owned");
            updatedApplication.PropertyAddress.Should().Be("123 Main St");
            updatedApplication.LoanAmount.Should().Be(100000);
            updatedApplication.AnnualIncome.Should().Be(50000);
            updatedApplication.CreditScore.Should().Be(700);
            updatedApplication.LoanTerm.Should().Be(15);
            updatedApplication.PropertyValue.Should().Be(200000);
            updatedApplication.MonthlyDebts.Should().Be(500);
        }

        [Fact]
        public async Task UpdateDraftLoanApplicationOnlyParticularFieldsTest()
        {
            var draftApplication = new LoanApplication
            {
                LoanStatus = "Draft",
                EmploymentStatus = "Part-time",
                ResidenceType = "Rented",
                PropertyAddress = "456 abc St",
                LoanAmount = 50000,
                AnnualIncome = 40000,
                CreditScore = 650,
                LoanTerm = 10,
                PropertyValue = 150000,
                MonthlyDebts = 300,
                ApplicationDate = DateTime.UtcNow,
                UserId = 2
            };
            _context.LoanApplications.Add(draftApplication);
            await _context.SaveChangesAsync();

            var updatedDto = new UpdateApplicationDto
                               (
                                   LoanAmount: null,
                                   AnnualIncome: null,
                                   EmploymentStatus: "Full-time",
                                   CreditScore: null,
                                   ResidenceType: null,
                                   LoanTerm: null,
                                   PropertyAddress: null,
                                   PropertyValue: null,
                                   MonthlyDebts: null
                               );

            var command = new UpdateDraftLoanApplicationCommand(draftApplication.Id, updatedDto);

            await _handler.Handle(command, CancellationToken.None);


            var updatedApplication = await _context.LoanApplications.FindAsync(draftApplication.Id);
            updatedApplication.EmploymentStatus.Should().Be("Full-time");
            updatedApplication.ResidenceType.Should().Be("Rented");
            updatedApplication.PropertyAddress.Should().Be("456 abc St");
            updatedApplication.LoanAmount.Should().Be(50000);
            updatedApplication.AnnualIncome.Should().Be(40000);
            updatedApplication.CreditScore.Should().Be(650);
            updatedApplication.LoanTerm.Should().Be(10);
            updatedApplication.PropertyValue.Should().Be(150000);
            updatedApplication.MonthlyDebts.Should().Be(300);
        }
        [Fact]
        public async Task ShouldNotUpdateDraftLoanApplicationWhenAllFieldsAreNullOrEmptyTest()
        {
            var draftApplication = new LoanApplication
            {
                LoanStatus = "Draft",
                EmploymentStatus = "Part-time",
                ResidenceType = "Rented",
                PropertyAddress = "456 abc St",
                LoanAmount = 50000,
                AnnualIncome = 40000,
                CreditScore = 650,
                LoanTerm = 10,
                PropertyValue = 150000,
                MonthlyDebts = 300,
                ApplicationDate = DateTime.UtcNow,
                UserId = 2
            };
            _context.LoanApplications.Add(draftApplication);
            await _context.SaveChangesAsync();

            var updatedDto = new UpdateApplicationDto
                               (
                                   LoanAmount: null,
                                   AnnualIncome: null,
                                   EmploymentStatus: null,
                                   CreditScore: null,
                                   ResidenceType: null,
                                   LoanTerm: null,
                                   PropertyAddress: null,
                                   PropertyValue: null,
                                   MonthlyDebts: null
                               );

            var command = new UpdateDraftLoanApplicationCommand(draftApplication.Id, updatedDto);

            await _handler.Handle(command, CancellationToken.None);


            var updatedApplication = await _context.LoanApplications.FindAsync(draftApplication.Id);
            updatedApplication.EmploymentStatus.Should().Be("Part-time");
            updatedApplication.ResidenceType.Should().Be("Rented");
            updatedApplication.PropertyAddress.Should().Be("456 abc St");
            updatedApplication.LoanAmount.Should().Be(50000);
            updatedApplication.AnnualIncome.Should().Be(40000);
            updatedApplication.CreditScore.Should().Be(650);
            updatedApplication.LoanTerm.Should().Be(10);
            updatedApplication.PropertyValue.Should().Be(150000);
            updatedApplication.MonthlyDebts.Should().Be(300);
        }

        [Fact]
        public async Task ShouldThrowExceptionWhenDraftLoanApplicationNotFoundTest()
        {
            var updatedDto = new UpdateApplicationDto
            (
                LoanAmount: 100000,
                AnnualIncome: 50000,
                EmploymentStatus: "Full-time",
                CreditScore: 700,
                ResidenceType: "Owned",
                LoanTerm: 15,
                PropertyAddress: "123 Main St",
                PropertyValue: 200000,
                MonthlyDebts: 500
            );

            var command = new UpdateDraftLoanApplicationCommand(999, updatedDto); 

            var act = () => _handler.Handle(command, CancellationToken.None);

            await act.Should().ThrowAsync<InvalidOperationException>().WithMessage("Loan draft not found.");
        }
        [Fact]
        public async Task ShouldThrowExceptionWhenLoanApplicationIsNotDraftTest()
        {
            var submittedApplication = new LoanApplication
            {
                LoanStatus = "Approved",
                EmploymentStatus = "Part-time",
                ResidenceType = "Rented",
                PropertyAddress = "456 Elm St",
                LoanAmount = 50000,
                AnnualIncome = 40000,
                CreditScore = 650,
                LoanTerm = 10,
                PropertyValue = 150000,
                MonthlyDebts = 300,
                ApplicationDate = DateTime.UtcNow,
                UserId = 2
            };
            _context.LoanApplications.Add(submittedApplication);
            await _context.SaveChangesAsync();

            var updatedDto = new UpdateApplicationDto
            (
                LoanAmount: 100000,
                AnnualIncome: 50000,
                EmploymentStatus: "Full-time",
                CreditScore: 700,
                ResidenceType: "Owned",
                LoanTerm: 15,
                PropertyAddress: "123 Main St",
                PropertyValue: 200000,
                MonthlyDebts: 500
            );

            var command = new UpdateDraftLoanApplicationCommand(submittedApplication.Id, updatedDto);

           var act = () => _handler.Handle(command, CancellationToken.None);

            await act.Should().ThrowAsync<InvalidOperationException>().WithMessage("Loan already submitted.");
        }
     }
}
