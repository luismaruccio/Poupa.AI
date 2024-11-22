using FluentValidation.TestHelper;
using Poupa.AI.Application.DTOs.Categories;
using Poupa.AI.Application.Validators.Categories;
using Poupa.AI.Domain.Enums;

namespace Poupa.AI.Application.Tests.Validators.Users
{
    public class UpdateCategoryRequestValidatorTests
    {
        private readonly UpdateCategoryRequestValidator _validator;

        public UpdateCategoryRequestValidatorTests()
        {
            _validator = new UpdateCategoryRequestValidator();
        }

        [Fact]
        public async Task UpdateCategoryRequestValidator_WhenIdIsEmpty_ShouldReturnError()
        {
            var request = new UpdateCategoryRequest(0, 1, "Category", TransactionType.Income);
            var validationResult = await _validator.TestValidateAsync(request);
            validationResult.ShouldHaveValidationErrorFor(c => c.Id);
        }

        [Fact]
        public async Task UpdateCategoryRequestValidator_WhenUserIdIsEmpty_ShouldReturnError()
        {
            var request = new UpdateCategoryRequest(1, 0, "Category", TransactionType.Income);
            var validationResult = await _validator.TestValidateAsync(request);
            validationResult.ShouldHaveValidationErrorFor(c => c.UserId);
        }

        [Fact]
        public async Task UpdateCategoryRequestValidator_WhenNameIsEmpty_ShouldReturnError()
        {
            var request = new UpdateCategoryRequest(1, 1, string.Empty, TransactionType.Income);
            var validationResult = await _validator.TestValidateAsync(request);
            validationResult.ShouldHaveValidationErrorFor(c => c.Name);
        }

        [Fact]
        public async Task UpdateCategoryRequestValidator_WhenTypeIsNone_ShouldReturnError()
        {
            var request = new UpdateCategoryRequest(1, 1, "Category", TransactionType.None);
            var validationResult = await _validator.TestValidateAsync(request);
            validationResult.ShouldHaveValidationErrorFor(c => c.Type);
        }

        [Fact]
        public async Task UpdateCategoryRequestValidator_WhenTypeIsntInEnum_ShouldReturnError()
        {
            var request = new UpdateCategoryRequest(1, 1, "Category", (TransactionType)999);
            var validationResult = await _validator.TestValidateAsync(request);
            validationResult.ShouldHaveValidationErrorFor(c => c.Type);
        }
    }
}
