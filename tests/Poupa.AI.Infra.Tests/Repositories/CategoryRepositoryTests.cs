using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using Poupa.AI.Domain.Entities;
using Poupa.AI.Domain.Enums;
using Poupa.AI.Domain.Interfaces.Repositories.Common;
using Poupa.AI.Infra.Data;
using Poupa.AI.Infra.Repositories;
using Poupa.AI.Infra.Tests.DataGenerators;
using Poupa.AI.Infra.Tests.InMemoryDB;
using System.Transactions;

namespace Poupa.AI.Infra.Tests.Repositories
{
    public class CategoryRepositoryTests
    {
        private readonly PoupaAIDbContext _context;
        private readonly Mock<ILogger<CategoryRepository>> _loggerMock = new();
        private readonly CategoryRepository _repository;
        private readonly UserGenerator _userGenerator = new();
        private readonly CategoryGenerator _categoryGenerator = new();


        public CategoryRepositoryTests()
        {
            var contextFactory = new InMemoryDBContextFactory();
            _context = contextFactory.GetContext();

            _repository = new CategoryRepository(_context, _loggerMock.Object);
        }

        [Fact]
        public async Task AddAsync_WhenAddingValidCategory_ShouldReturnSuccess()
        {
            var userId = await InsertUser();
            var category = _categoryGenerator.GetFakeCategory();
            category.UserId = userId;
            var result = await _repository.AddAsync(category);

            result.IsSuccess.Should().BeTrue();
            result.Success.Should().NotBeNull();
            result.Success!.Id.Should().NotBe(0);
        }

        [Fact]
        public async Task AddAsync_WhenAnExceptionIsThrown_ShouldReturnFailure()
        {
            var category = _categoryGenerator.GetFakeCategory();
            category.UserId = 1000;

            var result = await _repository.AddAsync(category);

            result.IsError.Should().BeTrue();
            result.Error.Should().NotBeNull();
        }

        [Fact]
        public async Task GetByIdAsync_WhenCategoryExists_ShouldReturnTheCategory()
        {
            var category = await InsertCategory();

            var result = await _repository.GetByIdAsync(category.Id);

            result.IsSuccess.Should().BeTrue();
            result.Success.Should().NotBeNull();
        }

        [Fact]
        public async Task GetByIdAsync_WhenCategoryNotExists_ShouldReturnFailure()
        {
            var result = await _repository.GetByIdAsync(1000);

            result.IsError.Should().BeTrue();
        }

        [Fact]
        public async Task GetAllByUserAsync_WhenHasCategories_ShouldReturnTheCategories()
        {
            var userIdA = await InsertUser();
            var userIdB = await InsertUser();

            for (var i = 0; i < 10; i++)
            {
                await InsertCategory(userIdA);
                await InsertCategory(userIdB);
            }

            var categories = await _repository.GetAllByUserAsync(userIdA);

            categories.IsSuccess.Should().BeTrue();
            categories.Success.Should().OnlyHaveUniqueItems();
            categories.Success!.Count.Should().Be(10);
        }

        [Fact]
        public async Task GetAllByUserAsync_WhenHasntCategories_ShouldReturnFailure()
        {
            var categories = await _repository.GetAllByUserAsync(100);

            categories.IsError.Should().BeTrue();
        }

        [Fact]
        public async Task GetAllByUserAndTypeAsync_WhenHasCategories_ShouldReturnTheCategories()
        {
            var userId = await InsertUser();

            for (var i = 0; i < 5; i++)
            {
                await InsertCategory(userId, TransactionType.Income);
                await InsertCategory(userId, TransactionType.Expense);
            }

            var categories = await _repository.GetAllByUserAndTypeAsync(userId, TransactionType.Income);

            categories.IsSuccess.Should().BeTrue();
            categories.Success.Should().OnlyHaveUniqueItems();
            categories.Success!.Count.Should().Be(5);
        }

        [Fact]
        public async Task GetAllByUserAndTypeAsync_WhenHasntCategories_ShouldReturnFailure()
        {
            var userId = await InsertUser();

            for (var i = 0; i < 5; i++)
            {
                await InsertCategory(userId, TransactionType.Income);
            }

            var categories = await _repository.GetAllByUserAndTypeAsync(userId, TransactionType.Expense);

            categories.IsError.Should().BeTrue();
        }

        [Fact]
        public async Task UpdateAsync_WhenCategoryExists_ShouldUpdateTheCategory()
        {
            var category = await InsertCategory();
            category!.Name = "Updated Category";

            var result = await _repository.UpdateAsync(category);

            result.IsSuccess.Should().BeTrue();
            result.Success.Should().NotBeNull();
        }

        [Fact]
        public async Task UpdateAsync_WhenCategoryNotExists_ShouldUpdateFailure()
        {
            var category = _categoryGenerator.GetFakeCategory();

            var result = await _repository.UpdateAsync(category);

            result.IsError.Should().BeTrue();
        }

        [Fact]
        public async Task DeleteAsync_WhenCategoryExists_ShouldSetCategoryDeleted()
        {
            var category = await InsertCategory();

            var result = await _repository.DeleteAsync(category.Id);

            result.IsSuccess.Should().BeTrue();
            result.Success.Should().NotBeNull();

            var deletedCategory = await _context.Categories.FindAsync(category.Id);            
            deletedCategory!.IsDeleted.Should().BeTrue();
        }

        [Fact]
        public async Task DeleteAsync_WhenCategoryNotExists_ShouldReturnFailure()
        {
            var result = await _repository.DeleteAsync(100);

            result.IsError.Should().BeTrue();
        }

        [Fact]
        public async Task DeleteAsync_WhenThrowsExceptionOnSave_ShouldReturnFailure()
        {
            var category = await InsertCategory();

            _context.Entry(category).State = EntityState.Deleted;

            using var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
            var result = await _repository.DeleteAsync(category.Id);

            transaction.Dispose();

            result.IsError.Should().BeTrue();
        }

        private async Task<int> InsertUser()
        {
            var fakeUser = _userGenerator.GetFakeUser();

            await _context.Users.AddAsync(fakeUser);
            await _context.SaveChangesAsync();

            return fakeUser.Id;
        }

        private async Task<Category> InsertCategory(int userId = 0, TransactionType? type = null)
        {
            if (userId == 0)
            {
                userId = await InsertUser();
            }

            var category = _categoryGenerator.GetFakeCategory();
            category.UserId = userId;
            if (type != null)
                category.Type = type.Value;
            await _context.Categories.AddAsync(category);
            await _context.SaveChangesAsync();

            return category;
        }

    }
}
