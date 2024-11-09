using FluentValidation.TestHelper;
using Poupa.AI.Application.DTOs.Users;
using Poupa.AI.Application.Validators.Users;

namespace Poupa.AI.Application.Tests.Validators.Users
{
    public class CreateUserRequestValidatorTests
    {
        private readonly CreateUserRequestValidator _validator;

        public CreateUserRequestValidatorTests()
        {
            _validator = new CreateUserRequestValidator();
        }

        [Fact]
        public async Task CreateUserRequestValidator_WhenNameIsEmpty_ShouldReturnError()
        {
            var request = new CreateUserRequest(string.Empty, "email@test.com", "password");
            var validationResult = await _validator.TestValidateAsync(request);
            validationResult.ShouldHaveValidationErrorFor(u => u.Name);
        }

        [Fact]
        public async Task CreateUserRequestValidator_WhenEmailIsEmpty_ShouldReturnError()
        {
            var request = new CreateUserRequest("Test", string.Empty, "password");
            var validationResult = await _validator.TestValidateAsync(request);
            validationResult.ShouldHaveValidationErrorFor(u => u.Email);
        }

        [Fact]
        public async Task CreateUserRequestValidator_WhenEmailIsInvalid_ShouldReturnError()
        {
            var request = new CreateUserRequest("Test", "invalid_email", "password");
            var validationResult = await _validator.TestValidateAsync(request);
            validationResult.ShouldHaveValidationErrorFor(u => u.Email);
        }

        [Fact]
        public async Task CreateUserRequestValidator_WhenPasswordIsEmpty_ShouldReturnError()
        {
            var request = new CreateUserRequest("Test", "email@test.com", string.Empty);
            var validationResult = await _validator.TestValidateAsync(request);
            validationResult.ShouldHaveValidationErrorFor(u => u.Password);
        }

        [Fact]
        public async Task CreateUserRequestValidator_WhenRequestIsValid_ShouldReturnSucess()
        {
            var request = new CreateUserRequest("Test", "email@test.com", "password");
            var validationResult = await _validator.TestValidateAsync(request);
            validationResult.ShouldNotHaveAnyValidationErrors();
        }
    }
}
