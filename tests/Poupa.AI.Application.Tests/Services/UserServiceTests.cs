using FluentAssertions;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.Extensions.Logging;
using Moq;
using Poupa.AI.Application.DTOs.Users;
using Poupa.AI.Application.Interfaces.Services;
using Poupa.AI.Application.Services;
using Poupa.AI.Common.Utils;
using Poupa.AI.Domain.Entities;
using Poupa.AI.Domain.Interfaces.Repositories;

namespace Poupa.AI.Application.Tests.Services
{
    public class UserServiceTests
    {
        private readonly Mock<IUserRepository> _userRepositoryMock = new();
        private readonly Mock<IValidator<CreateUserRequest>> _createUserRequestMock = new();
        private readonly Mock<ILogger<UserService>> _loggerMock = new();
        private readonly Mock<IHashService> _hashService = new();
        private readonly UserService _userService;

        public UserServiceTests()
        {
            _userService = new UserService(_userRepositoryMock.Object, _createUserRequestMock.Object, _hashService.Object, _loggerMock.Object);
        }

        #region CreateUser

        [Fact]
        public async Task CreateUserAsync_WhenRequestIsInvalid_ShouldReturnFailure()
        {
            ConfigureCreateUserValidator(success: false);
            var request = new CreateUserRequest(string.Empty, "test@test.com", "password");

            var result = await _userService.CreateUserAsync(request);

            result.IsError.Should().BeTrue();
        }

        [Fact]
        public async Task CreateUserAsync_WhenEmailAlreadyInUse_ShouldReturnFailure()
        {
            ConfigureCreateUserValidator();
            _userRepositoryMock.Setup(x => x.GetByEmailAsync(It.IsAny<string>())).ReturnsAsync(
                Either<string, User>.FromSuccess(
                    new User()
                    {
                        Id = 1,
                        Name = "Test",
                        Email = "test@test.com",
                        Password = "password"
                    }
                )
            );

            var request = new CreateUserRequest("Test", "test@test.com", "password");

            var result = await _userService.CreateUserAsync(request);

            result.IsError.Should().BeTrue();
        }

        [Fact]
        public async Task CreateUserAsync_WhenFailureToAddUser_ShouldReturnFailure()
        {
            ConfigureCreateUserValidator();
            _userRepositoryMock.Setup(x => x.GetByEmailAsync(It.IsAny<string>())).ReturnsAsync(
                Either<string, User>.FromError("User not found by email")
            );

            _userRepositoryMock.Setup(x => x.AddAsync(It.IsAny<User>())).ReturnsAsync(
                Either<string, User>.FromError("Generic Error")
            );

            var request = new CreateUserRequest("Test", "test@test.com", "password");

            var result = await _userService.CreateUserAsync(request);

            result.IsError.Should().BeTrue();
        }

        [Fact]
        public async Task CreateUserAsync_WhenExecuteWithSuccess_ShouldReturnResponse()
        {
            var request = new CreateUserRequest("Test", "test@test.com", "password");
            var createdUser = new User() { Id = 1, Name = "Test", Email = "test@test.com", Password = "password" };                            

            ConfigureCreateUserValidator();
            _userRepositoryMock.Setup(x => x.GetByEmailAsync(It.IsAny<string>())).ReturnsAsync(
                Either<string, User>.FromError("User not found by email")
            );

            _userRepositoryMock.Setup(x => x.AddAsync(It.IsAny<User>())).ReturnsAsync(
                Either<string, User>.FromSuccess(createdUser)
            );

            var result = await _userService.CreateUserAsync(request);

            result.IsSuccess.Should().BeTrue();
            result.Success.Should().BeOfType<CreateUserResponse>();
        }

        #endregion

        #region Privates

        private void ConfigureCreateUserValidator(bool success = true)
        {
            var validationResult = new ValidationResult()
            {
                Errors = success ? [] : [new("name", "empty")]
            };

            _createUserRequestMock.Setup(x => x.Validate(It.IsAny<CreateUserRequest>())).Returns(validationResult);
        }


        #endregion

    }
}
