using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Poupa.AI.API.Controllers;
using Poupa.AI.Application.DTOs.Common;
using Poupa.AI.Application.DTOs.Users;
using Poupa.AI.Application.Interfaces.Services;
using Poupa.AI.Common.Utils;

namespace Poupa.AI.API.Tests.Controllers
{
    public class UserControllerTests
    {
        private readonly Mock<IUserService> _userServiceMock = new();
        private readonly Mock<ILogger<UserController>> _loggerMock = new();
        private readonly UserController _controller;

        public UserControllerTests()
        {
            _controller = new UserController(_userServiceMock.Object, _loggerMock.Object);
        }

        #region AddUser

        [Fact]
        public async Task AddUser_WhenTheUserWasCreatedSuccessfully_ShouldReturnOk()
        {
            _userServiceMock.Setup(x => x.CreateUserAsync(It.IsAny<CreateUserRequest>())).ReturnsAsync(
                Either<FailureResponse, CreateUserResponse>.FromSuccess(
                    new CreateUserResponse(
                        id: 1, 
                        name: "Test", 
                        email: "test@test.com"
                    )
                )
            );

            var result = await _controller.AddUserAsync(
                new CreateUserRequest(
                    name: "Test",
                    email: "test@test.com",
                    password: "password"
                )
            );

            result.Should().BeOfType<OkObjectResult>();
        }

        [Fact]
        public async Task AddUser_WhenThereWasFailureToCreateTheUser_ShouldReturnBadRequest()
        {
            _userServiceMock.Setup(x => x.CreateUserAsync(It.IsAny<CreateUserRequest>())).ReturnsAsync(
                Either<FailureResponse, CreateUserResponse>.FromError(
                    new FailureResponse(
                        error: "Generic Error"
                    )
                )
            );

            var result = await _controller.AddUserAsync(
                new CreateUserRequest(
                    name: "Test",
                    email: "test@test.com",
                    password: "password"
                )
            );

            result.Should().BeOfType<BadRequestObjectResult>();
        }

        #endregion
    }
}
