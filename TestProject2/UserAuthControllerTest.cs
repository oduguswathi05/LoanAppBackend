using FluentAssertions;
using LoanApp.Controllers;
using LoanApp.Features.Commands.Create.Users;
using LoanApp.Features.DTOS;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestProject2
{
    public class UserAuthControllerTest
    {
        private readonly Mock<IMediator> _mediatorMock;
        private readonly UserAuthController _controller;

        public UserAuthControllerTest()
        {
            _mediatorMock = new Mock<IMediator>();
            _controller = new UserAuthController(_mediatorMock.Object);
        }
        [Fact]
        public async Task Login_ShouldReturnOk_WhenCredentialsAreValid()
        {
            var loginDto = new UserLoginDto
            (
                Email : "test@example.com",
                PasswordHash : "hashedpassword123"
            );

            var expectedResponse = new LoginResponseDto(
                                                            Token: "mocked-jwt-token",
                                                            Role: "Customer",
                                                            UserId: 42
                                                        );

            _mediatorMock.Setup(m => m.Send(It.IsAny<LoginUserCommand>(), default)).ReturnsAsync(expectedResponse);


            var result = await _controller.Login(loginDto);

            result.Should().BeOfType<OkObjectResult>().Which.Value.Should().Be(expectedResponse);
        }

        [Fact]
        public async Task Login_ShouldReturnUnauthorized_WhenLoginFails()
        {
            var loginDto = new UserLoginDto
            (
                Email : "wrong@example.com",
                PasswordHash : "wronghash"
            );

            _mediatorMock.Setup(m => m.Send(It.IsAny<LoginUserCommand>(), default)).ReturnsAsync((LoginResponseDto?)null);

            var result = await _controller.Login(loginDto);

            result.Should().BeOfType<UnauthorizedObjectResult>().Which.Value.Should().Be("Invalid email or password.");
        }


    }
}
