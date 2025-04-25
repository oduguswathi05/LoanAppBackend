using FluentAssertions;
using LoanApp.Data;
using LoanApp.Features.Commands.Create.LoanApplications;
using LoanApp.Features.DTOS;
using LoanApp.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace TestProject2
{
    public class CreateDraftLoanApplicationHandlerTests
    {
        private readonly DbContextOptions<ApplicationDbContext> _dbContextOptions;
        private readonly ApplicationDbContext _context;
        private readonly CreateDraftLoanApplicationCommandHandler _handler;

        public CreateDraftLoanApplicationHandlerTests()
        {
            _dbContextOptions = new DbContextOptionsBuilder<ApplicationDbContext>().UseInMemoryDatabase(databaseName: "TestDatabase").Options;
            _context = new ApplicationDbContext(_dbContextOptions);
            _handler = new CreateDraftLoanApplicationCommandHandler(_context);
        }

       
        [Fact]
        public async Task SaveDraftAndReturnIdWhenUserHasNoApprovedApp()
        {
            var command = new CreateDraftLoanApplicationCommand(GetValidDraftDto(),userId:2);
            var result = await _handler.Handle(command,default);
            var saved = await _context.LoanApplications.FindAsync(result);
            //saved.Id.Should().Be(1);
            saved.Should().NotBeNull();
            saved.Id.Should().BeGreaterThan(0);
        }

        [Fact]
        public async Task ThrowsExceptionWhenUserHasApprovedApp()
        {
            _context.LoanApplications.Add(new LoanApplication
            {
                UserId = 1,
                LoanStatus = "Approved",
                EmploymentStatus = "Employed",       
                PropertyAddress = "123 Main St",     
                ResidenceType = "Owned"             
            });
            await _context.SaveChangesAsync();

            var command = new CreateDraftLoanApplicationCommand(GetValidDraftDto(), userId: 1);

           var act = async () => await _handler.Handle(command, CancellationToken.None);

            await act.Should().ThrowAsync<Exception>().WithMessage("Your Application already accepted and You can submit only one application");
        }

        private LoanApplicationDraftDto GetValidDraftDto()
        {
           return new LoanApplicationDraftDto()
            {
                LoanAmount = 100000,
                AnnualIncome = 75000,
                EmploymentStatus = "Employed",
                CreditScore = 700,
                ResidenceType = "Owned",
                LoanTerm = 15,
                PropertyAddress = "123 Main St",
                PropertyValue = 150000,
                MonthlyDebts = 500
            };
        }
       


    }
}
