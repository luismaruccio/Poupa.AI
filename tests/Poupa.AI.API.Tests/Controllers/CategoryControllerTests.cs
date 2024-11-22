using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Poupa.AI.API.Controllers;
using Poupa.AI.Application.DTOs.Categories;
using Poupa.AI.Application.DTOs.Common;
using Poupa.AI.Application.Interfaces.Services;
using Poupa.AI.Common.Utils;
using Poupa.AI.Domain.Enums;

namespace Poupa.AI.API.Tests.Controllers
{
    public class CategoryControllerTests
    {
        private readonly Mock<ICategoryService> _categoryServiceMock = new();
        private readonly Mock<ILogger<CategoryController>> _loggerMock = new();
        private readonly CategoryController _categoryController;

        public CategoryControllerTests()
        {
            _categoryController = new CategoryController(_categoryServiceMock.Object, _loggerMock.Object);
        }

        #region AddCategoryAsync

        [Fact]
        public async Task AddCategoryAsync_WhenTheCategoryWasCreatedSuccessfully_ShouldReturnCreated()
        {
            _categoryServiceMock.Setup(x => x.CreateCategoryAsync(It.IsAny<CreateCategoryRequest>())).ReturnsAsync(
                Either<MessageResponse, CategoryResponse>.FromSuccess(
                    new CategoryResponse(
                        id: 1,
                        userId: 1,
                        name: "Test",
                        type: TransactionType.Income
                    )
                )
            );

            var result = await _categoryController.AddCategoryAsync(
                new CreateCategoryRequest(
                    name: "Test",
                    type: TransactionType.Income,
                    userId: 1
                )
            );

            result.Should().BeOfType<CreatedResult>();
        }

        [Fact]
        public async Task AddCategoryAsync_WhenThereWasFailureToCreateTheCategory_ShouldReturnBadRequest()
        {
            _categoryServiceMock.Setup(x => x.CreateCategoryAsync(It.IsAny<CreateCategoryRequest>())).ReturnsAsync(
                Either<MessageResponse, CategoryResponse>.FromError(
                    new MessageResponse(
                        message: "Generic Error"
                    )
                )
            );

            var result = await _categoryController.AddCategoryAsync(
                new CreateCategoryRequest(
                    name: "Test",
                    type: TransactionType.Income,
                    userId: 1
                )
            );

            result.Should().BeOfType<BadRequestObjectResult>();
        }

        #endregion

        #region UpdateCategoryAsync

        [Fact]
        public async Task UpdateCategoryAsync_WhenTheCategoryWasUpdatedSuccessfully_ShouldReturnOk()
        {
            _categoryServiceMock.Setup(x => x.UpdateCategoryAsync(It.IsAny<UpdateCategoryRequest>())).ReturnsAsync(
                Either<MessageResponse, CategoryResponse>.FromSuccess(
                    new CategoryResponse(
                        id: 1,
                        userId: 1,
                        name: "Test",
                        type: TransactionType.Income
                    )
                )
            );

            var result = await _categoryController.UpdateCategoryAsync(
                new UpdateCategoryRequest(
                    id: 1,
                    userId: 1,
                    name: "Test",
                    type: TransactionType.Income
                )
            );

            result.Should().BeOfType<OkObjectResult>();
        }

        [Fact]
        public async Task UpdateCategoryAsync_WhenThereWasFailureToUpdateTheCategory_ShouldReturnBadRequest()
        {
            _categoryServiceMock.Setup(x => x.UpdateCategoryAsync(It.IsAny<UpdateCategoryRequest>())).ReturnsAsync(
                Either<MessageResponse, CategoryResponse>.FromError(
                    new MessageResponse(
                        message: "Generic Error"
                    )
                )
            );

            var result = await _categoryController.UpdateCategoryAsync(
                new UpdateCategoryRequest(
                    id: 1,
                    userId: 1,
                    name: "Test",
                    type: TransactionType.Income
                )
            );

            result.Should().BeOfType<BadRequestObjectResult>();
        }

        #endregion

        #region DeleteCategoryAsync

        [Fact]
        public async Task DeleteCategoryAsync_WhenTheCategoryWasDeletedSuccessfully_ShouldReturnOk()
        {
            _categoryServiceMock.Setup(x => x.DeleteCategoryAsync(It.IsAny<int>())).ReturnsAsync(
                Either<MessageResponse, MessageResponse>.FromSuccess(
                    new MessageResponse(
                        message: "Success"
                    )
                )
            );

            var result = await _categoryController.DeleteCategoryAsync(id: 1);

            result.Should().BeOfType<OkObjectResult>();
        }

        [Fact]
        public async Task DeleteCategoryAsync_WhenThereWasFailureToDeleteTheCategory_ShouldReturnBadRequest()
        {
            _categoryServiceMock.Setup(x => x.DeleteCategoryAsync(It.IsAny<int>())).ReturnsAsync(
                Either<MessageResponse, MessageResponse>.FromError(
                    new MessageResponse(
                        message: "Fail"
                    )
                )
            );

            var result = await _categoryController.DeleteCategoryAsync(id: 1);

            result.Should().BeOfType<BadRequestObjectResult>();
        }

        #endregion

        #region GetCategoryByIdAsync

        [Fact]
        public async Task GetCategoryByIdAsync_WhenCategoryExists_ShouldReturnOk()
        {
            _categoryServiceMock.Setup(x => x.GetCategoryByIdAsync(It.IsAny<int>())).ReturnsAsync(
                Either<MessageResponse, CategoryResponse>.FromSuccess(
                    new CategoryResponse(
                        id: 1,
                        userId: 1,
                        name: "Test",
                        type: TransactionType.Income
                    )
                )
            );

            var result = await _categoryController.GetCategoryByIdAsync(id: 1);

            result.Should().BeOfType<OkObjectResult>();
        }

        [Fact]
        public async Task GetCategoryByIdAsync_WhenCategoryDoesNotExist_ShouldReturnBadRequest()
        {
            _categoryServiceMock.Setup(x => x.GetCategoryByIdAsync(It.IsAny<int>())).ReturnsAsync(
                Either<MessageResponse, CategoryResponse>.FromError(
                    new MessageResponse(
                        message: "Fail"
                    )
                )
            );

            var result = await _categoryController.GetCategoryByIdAsync(id: 1);

            result.Should().BeOfType<BadRequestObjectResult>();
        }

        #endregion

        #region GetCategoriesByUserAsync

        [Fact]
        public async Task GetCategoriesByUserAsync_WhenUserHasCategories_ShouldReturnOk()
        {
            _categoryServiceMock.Setup(x => x.GetCategoriesByUserAsync(It.IsAny<int>())).ReturnsAsync(
                Either<MessageResponse, CategoryBreakdownResponse>.FromSuccess(
                    new CategoryBreakdownResponse(
                        incomes: [],
                        expenses: [])
                )
            );

            var result = await _categoryController.GetCategoriesByUserAsync(userId: 1);

            result.Should().BeOfType<OkObjectResult>();
        }

        [Fact]
        public async Task GetCategoriesByUserAsync_WhenUserHasNoCategories_ShouldReturnBadRequest()
        {
            _categoryServiceMock.Setup(x => x.GetCategoriesByUserAsync(It.IsAny<int>())).ReturnsAsync(
                Either<MessageResponse, CategoryBreakdownResponse>.FromError(
                    new MessageResponse(
                        message: "Fail"
                    )
                )
            );

            var result = await _categoryController.GetCategoriesByUserAsync(userId: 1);

            result.Should().BeOfType<BadRequestObjectResult>();
        }

        #endregion

        #region GetCategoriesByUserAndTypeAsync

        [Fact]
        public async Task GetCategoriesByUserAndTypeAsync_WhenUserHasCategoriesByType_ShouldReturnOk()
        {
            _categoryServiceMock.Setup(x => x.GetCategoriesByUserAndTypeAsync(It.IsAny<int>(), It.IsAny<TransactionType>())).ReturnsAsync(
                Either<MessageResponse, IReadOnlyList<CategoryResponse>>.FromSuccess(
                    []
                )
            );

            var result = await _categoryController.GetCategoriesByUserAndTypeAsync(userId: 1, type: TransactionType.Income);

            result.Should().BeOfType<OkObjectResult>();
        }

        [Fact]
        public async Task GetCategoriesByUserAndTypeAsync_WhenUserHasNoCategoriesByType_ShouldReturnBadRequest()
        {
            _categoryServiceMock.Setup(x => x.GetCategoriesByUserAndTypeAsync(It.IsAny<int>(), It.IsAny<TransactionType>())).ReturnsAsync(
                Either<MessageResponse, IReadOnlyList<CategoryResponse>>.FromError(
                    new MessageResponse(
                        message: "Fail"
                    )
                )
            );

            var result = await _categoryController.GetCategoriesByUserAndTypeAsync(userId: 1, type: TransactionType.Income);

            result.Should().BeOfType<BadRequestObjectResult>();
        }

        #endregion

    }
}
