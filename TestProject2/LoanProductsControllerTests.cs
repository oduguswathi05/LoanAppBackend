using FluentAssertions;
using LoanApp.Controllers;
using LoanApp.Features.Commands.Create.LoanProducts;
using LoanApp.Features.Commands.Create.Users;
using LoanApp.Features.Commands.Delete;
using LoanApp.Features.Commands.Update.LoanProducts;
using LoanApp.Features.DTOS;
using LoanApp.Features.Queries.Get.Users;
using LoanApp.Features.Queries.List.LoanProducts;
using LoanApp.Features.Queries.List.Users;
using LoanApp.Models;
using LoanApp.Services;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace TestProject2
{
    public class LoanProductsControllerTests
    {
        private readonly Mock<IMediator> _mediatorMock;
        private readonly LoanProductsController _controller;
        private readonly ProductEligibilityCheckService _service;

        public LoanProductsControllerTests() {
            _mediatorMock = new Mock<IMediator>();
            _controller = new LoanProductsController(_mediatorMock.Object);
            _service = new ProductEligibilityCheckService();
         }

        [Fact]
        public async Task AddLoanProductTest()
        {
            var products = new LoanProductDto { ProductName = "ARM",InterestRate=5.5 };
             _mediatorMock.Setup(m => m.Send(It.IsAny<CreateLoanProductCommand>(),default)).ReturnsAsync(1);
            var result = await _controller.Add(products);
            result.Should().BeOfType<OkObjectResult>().Which.Value.Should().Be(1);
        }

        [Fact]
        public async Task GetAllLoanProductsTest()
        {
            var products = new List<LoanProduct> { new LoanProduct { ProductName = "ARM", InterestRate = 5.5 }, new LoanProduct { ProductName = "personal loan", InterestRate = 6.5 } };
            _mediatorMock.Setup(m => m.Send(It.IsAny<ListLoanProductsQuery>(), default)).ReturnsAsync(products);
            var result = await _controller.Get();
            result.Should().BeOfType<OkObjectResult>().Which.Value.Should().Be(products);
        }

        [Fact]
        public async Task DeleteLoanProductReturnsNotFoundTest()
        {
                
            _mediatorMock.Setup(m => m.Send(It.IsAny<DeleteLoanProductCommand>(), default)).Returns(Task.CompletedTask);
            var result = await _controller.Delete(1);
            result.Should().BeOfType<NoContentResult>();
        }
        [Fact]
        public async Task DeleteLoanProductReturnsBadRequestWhenExceptionThrownTest()
        {
            _mediatorMock.Setup(m => m.Send(It.IsAny<DeleteLoanProductCommand>(), default)).ThrowsAsync(new Exception("Product not found"));
            var result = await _controller.Delete(1);
            result.Should().BeOfType<BadRequestObjectResult>().Which.Value.Should().Be("Product not found");
        }

        [Fact]
        public async Task UpdateLoanProductReturnsNoContentWhenLoanProductUpdated()
        {
            var dto = new UpdateLoanProductDto(
                                                ProductName: null,
                                                InterestRate: null,
                                                MinLoanAmount: null,
                                                MaxLoanAmount: 3000000,
                                                MinLoanTerm: null,
                                                MaxLoanTerm: null
                                            );
            _mediatorMock.Setup(m => m.Send(It.IsAny<UpdateLoanProductCommand>(),default)).Returns(Task.CompletedTask);
            var result = await _controller.Update(1,dto);
            result.Should().BeOfType<NoContentResult>();
        }
        [Fact]
        public async Task UpdateLoanProductReturns500WhenExceptionThrownTest()
        {
            var dto = new UpdateLoanProductDto(
                                                ProductName: null,
                                                InterestRate: null,
                                                MinLoanAmount: null,
                                                MaxLoanAmount: 3000000,
                                                MinLoanTerm: null,
                                                MaxLoanTerm: null
                                            );
            _mediatorMock.Setup(m => m.Send(It.IsAny<UpdateLoanProductCommand>(), default)).ThrowsAsync(new Exception("Db Error"));
            var result = await _controller.Update(1,dto);
            //result.Should().BeOfType<ObjectResult>().Which.Value.Should().Be("Something went wrong");
            result.Should().BeOfType<ObjectResult>().Which.Value.ToString().Should().Contain("Something went wrong");
            result.Should().BeOfType<ObjectResult>().Which.Value.ToString().Should().Contain("Db Error");

        }

        [Fact]
        public async Task ShouldReturnsEligibleWithAdjustedRate()
        {
            var application = new LoanApplication
            {
                LoanAmount = 100000,
                PropertyValue = 150000,
                MonthlyDebts = 500,
                AnnualIncome = 60000,
                CreditScore = 720
            };

            var product = new LoanProductDto
            {
                ProductName = "Standard Loan",
                InterestRate = 5.0,
                MinLoanAmount = 50000,
                MaxLoanAmount = 300000,
                MinLoanTerm = 12,
                MaxLoanTerm = 60,
                MinCreditScore = 700,
                MinAnnualIncome = 40000
            };

            var result = _service.Evaluate(application, product);

            result.ProductName.Should().Be(product.ProductName);
            result.EligibilityMessage.Should().Be("✅ Eligible");
            result.AdjustedInterestRate.Should().Be(5.25);

        }

        [Fact]
        public async Task ShouldRejectWhenCreditScoreTooLow()
        {
            var application = new LoanApplication
            {
                LoanAmount = 100000,
                PropertyValue = 150000,
                MonthlyDebts = 500,
                AnnualIncome = 60000,
                CreditScore = 600
            };

            var product = new LoanProductDto
            {
                ProductName = "Low Rate Loan",
                InterestRate = 4.0,
                MinLoanAmount = 50000,
                MaxLoanAmount = 200000,
                MinLoanTerm = 12,
                MaxLoanTerm = 60,
                MinCreditScore = 650,
                MinAnnualIncome = 50000
            };
            
            var result = _service.Evaluate(application, product);
            result.EligibilityMessage.Should().Be("❌ Credit score too low");

        }
        [Fact]
        public async Task ShouldRejectWhenIncomeIsTooLow()
        {
            var application = new LoanApplication
            {
                LoanAmount = 100000,
                PropertyValue = 150000,
                MonthlyDebts = 500,
                AnnualIncome = 30000,
                CreditScore = 700
            };

            var product = new LoanProductDto
            {
                ProductName = "Premium Loan",
                InterestRate = 4.5,
                MinLoanAmount = 50000,
                MaxLoanAmount = 250000,
                MinLoanTerm = 12,
                MaxLoanTerm = 60,
                MinCreditScore = 650,
                MinAnnualIncome = 40000
            };

            var result = _service.Evaluate(application, product);
            result.EligibilityMessage.Should().Be("❌ Income too low");

        }
        [Theory]
        [InlineData(75,5.0)]
        [InlineData(81, 5.5)]
        [InlineData(91, 6.0)]

        public async Task ShouldAdjustRateBasedOnLTVRate(double ltvRate,double expectedRate)
        {
            var loanAmount = 100000;
            var propertyValue = loanAmount / (ltvRate / 100);
            var application = new LoanApplication
            {
                LoanAmount = loanAmount,
                PropertyValue = propertyValue,
                MonthlyDebts = 500,
                AnnualIncome = 60000,
                CreditScore = 750
            };

            var product = new LoanProductDto
            {
                ProductName = "Premium Loan",
                InterestRate = 5.0,
                MinLoanAmount = 50000,
                MaxLoanAmount = 300000,
                MinLoanTerm = 12,
                MaxLoanTerm = 60,
                MinCreditScore = 650,
                MinAnnualIncome = 40000
            };
            var result = _service.Evaluate(application, product);
            result.AdjustedInterestRate.Should().Be(expectedRate);
        }

        [Theory]
        [InlineData(20, 5.0)]
        [InlineData(37, 5.25)]
        [InlineData(45, 5.5)]

        public async Task ShouldAdjustRateBasedOnDTiRate(double dtiRate, double expectedRate)
        {
            var annualIncome = 60000;
            var monthlyDebts = (dtiRate / 100) * (annualIncome / 12);
            var application = new LoanApplication
            {
                LoanAmount = 100000,
                PropertyValue = 200000,
                MonthlyDebts = monthlyDebts,
                AnnualIncome = annualIncome,
                CreditScore = 750
            };

            var product = new LoanProductDto
            {
                ProductName = "Premium Loan",
                InterestRate = 5.0,
                MinLoanAmount = 50000,
                MaxLoanAmount = 300000,
                MinLoanTerm = 12,
                MaxLoanTerm = 60,
                MinCreditScore = 650,
                MinAnnualIncome = 40000
            };
            var result = _service.Evaluate(application, product);
            result.AdjustedInterestRate.Should().Be(expectedRate);
        }

    }
}