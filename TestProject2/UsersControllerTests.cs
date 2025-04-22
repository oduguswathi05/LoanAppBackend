using FluentAssertions;
using LoanApp.Controllers;
using LoanApp.Features.Commands.Create.Users;
using LoanApp.Features.DTOS;
using LoanApp.Features.Queries.Get.Users;
using LoanApp.Features.Queries.List.Users;
using LoanApp.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace TestProject2
{
    public class UsersControllerTests
    {
        private readonly Mock<IMediator> _mediatorMock;
        private readonly UsersController _controller;

        public UsersControllerTests() {
            _mediatorMock = new Mock<IMediator>();
            _controller = new UsersController(_mediatorMock.Object);
         }

        [Fact]
        public async Task GetAllUsersForLoanOfficer()
        {
            var users = new List<User> { new User { Id = 1, FirstName = "Test", Email = "test@example.com" } };
            _mediatorMock.Setup(m => m.Send(It.IsAny<ListUsersQuery>(), It.IsAny<CancellationToken>())).ReturnsAsync(users);

            var result = await _controller.Get();
            result.Should().BeOfType<OkObjectResult>().Which.Value.Should().Be(users);
        }

        [Fact]
        public async Task RegisterCReturnsOkWithNewCustomerCreated()
        {
            var user = new UserRegisterDto { Email = "test@example.com" };

            _mediatorMock.Setup(m => m.Send(It.IsAny<CreateCustomerCommand>(), It.IsAny<CancellationToken>())).ReturnsAsync(1);
            var result = await _controller.RegisterC(user);

            result.Should().BeOfType<OkObjectResult>().Which.Value.Should().Be(1);
        }

        [Fact]
        public async Task RegisterCReturnsBadRequestForEmaillreadyExist()
        {
            var user = new UserRegisterDto { Email = "test@example.com" };

            _mediatorMock.Setup(m => m.Send(It.IsAny<CreateCustomerCommand>(), It.IsAny<CancellationToken>())).ReturnsAsync((int?)null);
            var result = await _controller.RegisterC(user);

            result.Should().BeOfType<BadRequestObjectResult>().Which.Value.Should().Be("Email is already in use.");
        }
        
        [Fact]
        public async Task RegisterOReturnsOkWithNewCustomerCreated()
        {
            var user = new UserRegisterDto { Email = "test@example.com" };

            _mediatorMock.Setup(m => m.Send(It.IsAny<CreateLoanOfficerCommand>(), It.IsAny<CancellationToken>())).ReturnsAsync(2);
            var result = await _controller.RegisterO(user);

            result.Should().BeOfType<OkObjectResult>().Which.Value.Should().Be(2);
        }

        [Fact]
        public async Task RegisterOReturnsBadRequestForEmaillreadyExist()
        {
            var user = new UserRegisterDto { Email = "test@example.com" };

            _mediatorMock.Setup(m => m.Send(It.IsAny<CreateLoanOfficerCommand>(), It.IsAny<CancellationToken>())).ReturnsAsync((int?)null);
            var result = await _controller.RegisterO(user);

            result.Should().BeOfType<BadRequestObjectResult>().Which.Value.Should().Be("Email is already in use.");
        }

        [Fact]
        public async Task GetUserByIdReturnsOkWhenUserExists()
        {
            var user = new User {Id=1,Email = "test@example.com" };

            _mediatorMock.Setup(m => m.Send(It.IsAny<GetUserByIdQuery>(), It.IsAny<CancellationToken>())).ReturnsAsync(user);
            var result = await _controller.Get(1);

            result.Should().BeOfType<OkObjectResult>().Which.Value.Should().Be(user);
        }

        [Fact]
        public async Task GetUserByIdReturnsNotFoundWhenUserNotExist()
        {
            var user = new User { Id = 1, Email = "test@example.com" };

            _mediatorMock.Setup(m => m.Send(It.IsAny<GetUserByIdQuery>(), It.IsAny<CancellationToken>())).ReturnsAsync((User)null);
            var result = await _controller.Get(1);

            result.Should().BeOfType<NotFoundResult>();
        }


    }
}