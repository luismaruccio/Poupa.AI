using Bogus;
using FluentAssertions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using Moq;
using Poupa.AI.Application.DTOs.Categories;
using Poupa.AI.Application.DTOs.Common;
using Poupa.AI.Application.Services;
using Poupa.AI.Common.Utils;
using Poupa.AI.Domain.Entities;
using Poupa.AI.Domain.Enums;
using Poupa.AI.Domain.Interfaces.Repositories;

namespace Poupa.AI.Application.Tests.Services
{
    public class CategoryServiceTests
    {
        private readonly Mock<ICategoryRepository> _categoryRepositoryMock = new();
        private readonly Mock<IValidator<CreateCategoryRequest>> _createCategoryRequestValidatorMock = new();
        private readonly Mock<IValidator<UpdateCategoryRequest>> _updateCategoryRequestValidator = new();
        private readonly Mock<ILogger<CategoryService>> _loggerMock = new();
        private readonly Faker<Category> _categoryFaker;
        private readonly CategoryService _categoryService;

        public CategoryServiceTests()
        {
            _categoryService = new CategoryService(
                _categoryRepositoryMock.Object, 
                _createCategoryRequestValidatorMock.Object,
                _updateCategoryRequestValidator.Object,
                _loggerMock.Object
            );

            _categoryFaker = new Faker<Category>()
                .RuleFor(c => c.Id, f => f.Random.Int(min: 1, max: 999))
                .RuleFor(c => c.UserId, f => f.Random.Int(min: 1, max: 999))
                .RuleFor(c => c.Name, f => f.Commerce.Categories(1).First())
                .RuleFor(c => c.Type, f => f.Random.Enum<TransactionType>());
        }

        #region CreateCategoryAsync

        [Fact]
        public async Task CreateCategoryAsync_WhenRequestIsInvalid_ShouldReturnFailure()
        {
            ConfigureCreateCategoryValidator(success: false);
            var request = GetCreateCategoryRequest();

            var result = await _categoryService.CreateCategoryAsync(request);

            result.IsError.Should().BeTrue();
        }

        [Fact]
        public async Task CreateCategoryAsync_WhenFailureToAddCategory_ShouldReturnFailure()
        {
            ConfigureCreateCategoryValidator();
            _categoryRepositoryMock.Setup(x => x.AddAsync(It.IsAny<Category>())).ReturnsAsync(
                Either<string, Category>.FromError("Generic Error")
            );

            var request = GetCreateCategoryRequest();

            var result = await _categoryService.CreateCategoryAsync(request);

            result.IsError.Should().BeTrue();
        }

        [Fact]
        public async Task CreateCategoryAsync_WhenExecuteWithSuccess_ShouldReturnResponse()
        {
            var request = GetCreateCategoryRequest();
            var createdCategory = GetCategory();

            ConfigureCreateCategoryValidator();

            _categoryRepositoryMock.Setup(x => x.AddAsync(It.IsAny<Category>())).ReturnsAsync(
                Either<string, Category>.FromSuccess(createdCategory)
            );

            var result = await _categoryService.CreateCategoryAsync(request);

            result.IsSuccess.Should().BeTrue();
            result.Success.Should().BeOfType<CategoryResponse>();
        }

        #endregion

        #region DeleteCategoryAsync

        [Fact]
        public async Task DeleteCategoryAsync_WhenRequestIsInvalid_ShouldReturnFailure()
        {
            var result = await _categoryService.DeleteCategoryAsync(0);

            result.IsError.Should().BeTrue();
        }

        [Fact]
        public async Task DeleteCategoryAsync_WhenFailureToDeleteCategory_ShouldReturnFailure()
        {
            _categoryRepositoryMock.Setup(x => x.DeleteAsync(It.IsAny<int>())).ReturnsAsync(
                Either<string, string>.FromError("Generic Error")
            );

            var result = await _categoryService.DeleteCategoryAsync(1);

            result.IsError.Should().BeTrue();
        }

        [Fact]
        public async Task DeleteCategoryAsync_WhenExecuteWithSuccess_ShouldReturnResponse()
        {
            _categoryRepositoryMock.Setup(x => x.DeleteAsync(It.IsAny<int>())).ReturnsAsync(
                Either<string, string>.FromSuccess("Deleted")
            );

            var result = await _categoryService.DeleteCategoryAsync(1);

            result.IsSuccess.Should().BeTrue();
            result.Success.Should().BeOfType<MessageResponse>();
        }

        #endregion

        #region GetCategoriesByUserAndTypeAsync

        [Fact]
        public async Task GetCategoriesByUserAndTypeAsync_WhenRequestIsInvalid_ShouldReturnFailure()
        {
            var result = await _categoryService.GetCategoriesByUserAndTypeAsync(0, TransactionType.None);

            result.IsError.Should().BeTrue();
        }

        [Fact]
        public async Task GetCategoriesByUserAndTypeAsync_WhenFailureToGetCategories_ShouldReturnFailure()
        {
            _categoryRepositoryMock.Setup(x => x.GetAllByUserAndTypeAsync(It.IsAny<int>(), It.IsAny<TransactionType>())).ReturnsAsync(
                Either<string, IReadOnlyList<Category>>.FromError("Generic Error")
            );

            var result = await _categoryService.GetCategoriesByUserAndTypeAsync(1, TransactionType.Income);

            result.IsError.Should().BeTrue();
        }

        [Fact]
        public async Task GetCategoriesByUserAndTypeAsync_WhenExecuteWithSuccess_ShouldReturnResponse()
        {
            var categories = GetCategories();
            _categoryRepositoryMock.Setup(x => x.GetAllByUserAndTypeAsync(It.IsAny<int>(), It.IsAny<TransactionType>())).ReturnsAsync(
                Either<string, IReadOnlyList<Category>>.FromSuccess(categories)
            );

            var result = await _categoryService.GetCategoriesByUserAndTypeAsync(1, TransactionType.Income);

            result.IsSuccess.Should().BeTrue();
        }

        #endregion

        #region GetCategoriesByUserAsync

        [Fact]
        public async Task GetCategoriesByUserAsync_WhenRequestIsInvalid_ShouldReturnFailure()
        {
            var result = await _categoryService.GetCategoriesByUserAsync(0);

            result.IsError.Should().BeTrue();
        }

        [Fact]
        public async Task GetCategoriesByUserAsync_WhenFailureToGetCategories_ShouldReturnFailure()
        {
            _categoryRepositoryMock.Setup(x => x.GetAllByUserAsync(It.IsAny<int>())).ReturnsAsync(
                Either<string, IReadOnlyList<Category>>.FromError("Generic Error")
            );

            var result = await _categoryService.GetCategoriesByUserAsync(1);

            result.IsError.Should().BeTrue();
        }

        [Fact]
        public async Task GetCategoriesByUserAsync_WhenExecuteWithSuccess_ShouldReturnResponse()
        {
            var categories = GetCategories();
            _categoryRepositoryMock.Setup(x => x.GetAllByUserAsync(It.IsAny<int>())).ReturnsAsync(
                Either<string, IReadOnlyList<Category>>.FromSuccess(categories)
            );

            var result = await _categoryService.GetCategoriesByUserAsync(1);

            result.IsSuccess.Should().BeTrue();
        }

        #endregion

        #region GetCategoryByIdAsync

        [Fact]
        public async Task GetCategoryByIdAsync_WhenRequestIsInvalid_ShouldReturnFailure()
        {
            var result = await _categoryService.GetCategoryByIdAsync(0);

            result.IsError.Should().BeTrue();
        }

        [Fact]
        public async Task GetCategoryByIdAsync_WhenFailureToGetTheCategory_ShouldReturnFailure()
        {
            _categoryRepositoryMock.Setup(x => x.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(
                Either<string, Category>.FromError("Generic Error")
            );

            var result = await _categoryService.GetCategoryByIdAsync(1);

            result.IsError.Should().BeTrue();
        }

        [Fact]
        public async Task GetCategoryByIdAsync_WhenExecuteWithSuccess_ShouldReturnResponse()
        {
            var category = GetCategory();
            _categoryRepositoryMock.Setup(x => x.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(
                Either<string, Category>.FromSuccess(category)
            );

            var result = await _categoryService.GetCategoryByIdAsync(1);

            result.IsSuccess.Should().BeTrue();
        }

        #endregion

        #region UpdateCategoryAsync

        [Fact]
        public async Task UpdateCategoryAsync_WhenRequestIsInvalid_ShouldReturnFailure()
        {
            ConfigureUpdateCategoryValidator(success: false);
            var request = GetUpdateCategoryRequest();

            var result = await _categoryService.UpdateCategoryAsync(request);

            result.IsError.Should().BeTrue();
        }

        [Fact]
        public async Task UpdateCategoryAsync_WhenFailureToUpdateCategory_ShouldReturnFailure()
        {
            ConfigureUpdateCategoryValidator();
            _categoryRepositoryMock.Setup(x => x.UpdateAsync(It.IsAny<Category>())).ReturnsAsync(
                Either<string, Category>.FromError("Generic Error")
            );

            var request = GetUpdateCategoryRequest();

            var result = await _categoryService.UpdateCategoryAsync(request);

            result.IsError.Should().BeTrue();
        }

        [Fact]
        public async Task UpdateCategoryAsync_WhenExecuteWithSuccess_ShouldReturnResponse()
        {
            var request = GetUpdateCategoryRequest();
            var updatedCategory = GetCategory();

            ConfigureUpdateCategoryValidator();

            _categoryRepositoryMock.Setup(x => x.UpdateAsync(It.IsAny<Category>())).ReturnsAsync(
                Either<string, Category>.FromSuccess(updatedCategory)
            );

            var result = await _categoryService.UpdateCategoryAsync(request);

            result.IsSuccess.Should().BeTrue();
            result.Success.Should().BeOfType<CategoryResponse>();
        }


        #endregion

        #region Privates

        private void ConfigureCreateCategoryValidator(bool success = true)
        {
            var validationResult = new FluentValidation.Results.ValidationResult()
            {
                Errors = success ? [] : [new("name", "empty")]
            };

            _createCategoryRequestValidatorMock.Setup(x => x.Validate(It.IsAny<CreateCategoryRequest>())).Returns(validationResult);
        }

        private static CreateCategoryRequest GetCreateCategoryRequest()
            => new("Category", TransactionType.Income, 1);

        private void ConfigureUpdateCategoryValidator(bool success = true)
        {
            var validationResult = new FluentValidation.Results.ValidationResult()
            {
                Errors = success ? [] : [new("name", "empty")]
            };

            _updateCategoryRequestValidator.Setup(x => x.Validate(It.IsAny<UpdateCategoryRequest>())).Returns(validationResult);
        }

        private static UpdateCategoryRequest GetUpdateCategoryRequest()
            => new(1, 1, "Category", TransactionType.Income);

        private Category GetCategory()
            => _categoryFaker.Generate();

        private IReadOnlyList<Category> GetCategories(int quantity = 10)
        {
            var categories = new List<Category>();

            for (int i = 0; i < quantity; i++)
            {
                categories.Add(GetCategory());
            }

            return categories;
        }

        #endregion


    }
}
