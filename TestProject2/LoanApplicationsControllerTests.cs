using FluentAssertions;
using LoanApp.Controllers;
using LoanApp.Features.Commands.Create.LoanApplications;
using LoanApp.Features.Commands.Update.LoanApplication;
using LoanApp.Features.Commands.Update.LoanApplications;
using LoanApp.Features.DTOS;
using LoanApp.Features.Queries.Get.LoanApplications;
using LoanApp.Features.Queries.List.LoanApplications;
using LoanApp.Models;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace TestProject2
{
    public class LoanApplicationsControllerTests
    {
        private readonly Mock<IMediator> _mediatorMock;
        private readonly LoanApplicationsController _controller;

        public LoanApplicationsControllerTests() { 
           _mediatorMock = new Mock<IMediator>();
            _controller = new LoanApplicationsController( _mediatorMock.Object );
            var userClaims = new List<Claim>
                                            {
                                                new Claim("userId", "1"),
                                                new Claim(ClaimTypes.Role, "Customer")
                                            };
            var identity = new ClaimsIdentity(userClaims, "TestAuth");
            var userPrincipal = new ClaimsPrincipal(identity);

            _controller.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext { User = userPrincipal }
            };
        }
        [Fact]
        public async Task PostShouldReturnOkWithId()
        {
            var loanApplication = GetValidDraftDto();

            _mediatorMock.Setup(m=>m.Send(It.IsAny<CreateDraftLoanApplicationCommand>(),default)).ReturnsAsync(1);
            var result = await _controller.Post(loanApplication);
            result.Should().BeOfType<OkObjectResult>().Which.Value.Should().Be(1);
        }
        [Fact]
        public async Task PostMissingUserIdClaimReturnsUnauthorized()
        {
            _controller.ControllerContext.HttpContext.User = new ClaimsPrincipal();
            var draftDto = GetValidDraftDto();

            var result = await _controller.Post(draftDto);

            result.Should().BeOfType<UnauthorizedObjectResult>().Which.Value.Should().Be("User ID claim is missing.");
        }
        [Fact]
        public async Task ShouldReturnBadRequestWhenExceptionthrown()
        {
            var loanApplication = GetValidDraftDto();

            _mediatorMock.Setup(m => m.Send(It.IsAny<CreateDraftLoanApplicationCommand>(), default)).ThrowsAsync(new Exception("Database error"));
            var result = await _controller.Post(loanApplication);
            result.Should().BeOfType<BadRequestObjectResult>().Which.Value.Should().Be("Database error");
        }

        [Fact]
        public async Task GetLoanApplicationsTest()
        {
            var loanApplications = new List<LoanApplicationsDto>
            {
                new LoanApplicationsDto { Id = 1, LoanAmount = 100000 },
                new LoanApplicationsDto { Id = 2, LoanAmount = 200000 }
            };
            _mediatorMock.Setup(m => m.Send(It.IsAny<ListLoanApplicationsQuery>(),default)).ReturnsAsync(loanApplications);
            var result = await _controller.Get();
            result.Should().BeOfType<OkObjectResult>().Which.Value.Should().Be(loanApplications);
        }
        [Fact]
        public async Task UpdateDraftLoanApplicationTest()
        {
            int id = 1;
            var dto = new UpdateApplicationDto(
                            LoanAmount: 180000,
                            AnnualIncome: null,
                            EmploymentStatus: null,
                            CreditScore: 700,
                            ResidenceType: null,
                            LoanTerm: 10,
                            PropertyAddress: null,
                            PropertyValue: 220000,
                            MonthlyDebts: null
                           );

            _mediatorMock.Setup(m => m.Send(It.IsAny<UpdateDraftLoanApplicationCommand>(), default)).Returns(Task.CompletedTask);
            var result = await _controller.Update(id,dto);
            result.Should().BeOfType<NoContentResult>();
        }
        [Fact]
        public async Task ShouldCreateLoanApplicationAndReturnApproved()
        {

            var dto = new SubmitLoanApplicationDto(
                    Id: null,
                    LoanAmount: 200000,
                    AnnualIncome: 100000,
                    EmploymentStatus: "Employed",
                    CreditScore: 760,
                    ResidenceType: "Owner-occupied",
                    LoanTerm: 30,
                    PropertyAddress: "123 Test Street",
                    PropertyValue: 300000,
                    MonthlyDebts: 1000,
                    UserId: 1
            );
            var expectedResult = new LoanApplicationResultDto(1, "Approved");
            _mediatorMock.Setup(m=>m.Send(It.IsAny<SubmitLoanApplicationCommand>(),default)).ReturnsAsync(expectedResult);
            var result = await _controller.SubmitLoan(dto);
            result.Should().BeOfType<OkObjectResult>().Which.Value.Should().Be(expectedResult);

        }
       [Fact]
       public async Task GetLoanApplicationsByUserIdTest()
        {
            var loanApplications = new List<LoanApplication>
                                {
                                    new LoanApplication { Id = 1, UserId = 1, LoanAmount = 100000 },
                                    new LoanApplication { Id = 2, UserId = 1, LoanAmount = 200000 }
                                };
            _mediatorMock.Setup(m => m.Send(It.IsAny<GetLoanApplicationsByUserIdQuery>(), default)).ReturnsAsync(loanApplications);
            var result = await _controller.GetLoanApplicationsByUserId();
            result.Should().BeOfType<OkObjectResult>().Which.Value.Should().Be(loanApplications);

        }
        [Fact]
        public async Task GetLoanApplicationsByIdTest()
        {
            var loanApplication = new LoanApplication { Id = 1, UserId = 1, LoanAmount = 100000 };
                                
            _mediatorMock.Setup(m => m.Send(It.IsAny<GetLoanApplicationsByIdQuery>(), default)).ReturnsAsync(loanApplication);
            var result = await _controller.GetLoanApplicationsById(1);
            result.Should().BeOfType<OkObjectResult>().Which.Value.Should().Be(loanApplication);

        }
        [Fact]
        public async Task UpdateLoanStatusTest()
        {
            var id = 1;
            var dto = new ReviewApplicationDto("Approved", "Looks good");

            _mediatorMock.Setup(m => m.Send(It.IsAny<ReviewLoanApplicationCommand>(), It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);

          
            var result = await _controller.UpdateLoanStatus(id, dto);

          
            result.Should().BeOfType<NoContentResult>();

        }

        private LoanApplicationDraftDto GetValidDraftDto()
        {
           return new LoanApplicationDraftDto
                    {
                        LoanAmount = 100000,
                        AnnualIncome = 75000,
                        EmploymentStatus = "Employed",
                        CreditScore = 720,
                        ResidenceType = "Owned",
                        LoanTerm = 15,
                        PropertyAddress = "123 Main St",
                        PropertyValue = 150000,
                        MonthlyDebts = 500
                    };
        }

    }
}
