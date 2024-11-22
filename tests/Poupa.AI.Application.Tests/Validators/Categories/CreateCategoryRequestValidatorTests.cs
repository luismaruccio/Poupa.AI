using FluentValidation.TestHelper;
using Poupa.AI.Application.DTOs.Categories;
using Poupa.AI.Application.Validators.Categories;
using Poupa.AI.Domain.Enums;

namespace Poupa.AI.Application.Tests.Validators.Categories
{
    public class CreateCategoryRequestValidatorTests
    {
        private readonly CreateCategoryRequestValidator _validator;

        public CreateCategoryRequestValidatorTests()
        { 
            _validator = new CreateCategoryRequestValidator();
        }

        [Fact]
        public async Task CreateCategoryRequestValidator_WhenNameIsEmpty_ShouldReturnError()
        {
            var request = new CreateCategoryRequest(string.Empty, TransactionType.Income, 1);
            var validationResult = await _validator.TestValidateAsync(request);
            validationResult.ShouldHaveValidationErrorFor(c => c.Name);
        }

        [Fact]
        public async Task CreateCategoryRequestValidator_WhenTypeIsNone_ShouldReturnError()
        {
            var request = new CreateCategoryRequest("Category", TransactionType.None, 1);
            var validationResult = await _validator.TestValidateAsync(request);
            validationResult.ShouldHaveValidationErrorFor(c => c.Type);
        }

        [Fact]
        public async Task CreateCategoryRequestValidator_WhenTypeIsntInEnum_ShouldReturnError()
        {
            var request = new CreateCategoryRequest("Category", (TransactionType)999, 1);
            var validationResult = await _validator.TestValidateAsync(request);
            validationResult.ShouldHaveValidationErrorFor(c => c.Type);
        }

        [Fact]
        public async Task CreateCategoryRequestValidator_WhenUserId_ShouldReturnError()
        {
            var request = new CreateCategoryRequest("Category", TransactionType.Income, 0);
            var validationResult = await _validator.TestValidateAsync(request);
            validationResult.ShouldHaveValidationErrorFor(c => c.UserId);
        }
    }
}
