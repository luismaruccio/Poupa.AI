using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using Poupa.AI.Common.Extensions.Messages;
using Poupa.AI.Domain.Entities;
using Poupa.AI.Domain.Messages.Common;
using Poupa.AI.Domain.Messages.Users;
using Poupa.AI.Infra.Data;
using Poupa.AI.Infra.Repositories;
using Poupa.AI.Infra.Tests.InMemoryDB;

namespace Poupa.AI.Infra.Tests.Repositories
{
    public class UserRepositoryTests : IDisposable
    {
        private readonly PoupaAIDbContext _context;
        private readonly Mock<ILogger<UserRepository>> _loggerMock = new();
        private readonly UserRepository _repository;
        private readonly User _user;

        public UserRepositoryTests()
        {
            var contextFactory = new InMemoryDBContextFactory();
            _context = contextFactory.GetContext();

            _repository = new UserRepository(_context, _loggerMock.Object);

            _user = new User()
            {
                Name = "Test",
                Email = "test@test.com",
                Password = "Test",
            };
        }

        [Fact]
        public async Task AddAsync_WhenAddingValidUser_ShouldReturnSuccess()
        {
            var insertedUser = await _repository.AddAsync(_user);

            insertedUser.IsSuccess.Should().BeTrue();
            insertedUser.Success.Should().NotBeNull();
            insertedUser.Success!.Id.Should().NotBe(0);
        }

        [Fact]
        public async Task AddAsync_WhenAnExceptionIsThrown_ShouldReturnFailure()
        {
            await _repository.AddAsync(_user);

            var insertedUser = await _repository.AddAsync(_user);

            insertedUser.IsError.Should().BeTrue();
            insertedUser.Error.Should().NotBeNull();
        }

        [Fact]
        public async Task GetByIdAsync_WhenUserExists_ShouldReturnTheUser()
        {
            await InsertUser();

            var retornedUser = await _repository.GetByIdAsync(_user.Id);

            retornedUser.IsSuccess.Should().BeTrue();
            retornedUser.Success.Should().NotBeNull();
        }

        [Fact]
        public async Task GetByEmailAsync_WhenUserExists_ShouldReturnTheUser()
        {
            await InsertUser();

            var retornedUser = await _repository.GetByEmailAsync(_user.Email);

            retornedUser.IsSuccess.Should().BeTrue();
            retornedUser.Success.Should().NotBeNull();
        }

        [Fact]
        public async Task GetByEmailAsync_WhenUserNotExists_ShouldReturnFailure()
        {
            var retornedUser = await _repository.GetByEmailAsync("another.email@email.com");

            retornedUser.IsError.Should().BeTrue();
            retornedUser.Error.Should().NotBeNull();
            retornedUser.Error!.Should().BeEquivalentTo(RepositoryMessages.EntityNotFoundBy.WithParameters([UserMessages.User, UserMessages.Email]));
        }

        [Fact]
        public async Task DeleteAsync_WhenUserExists_ShouldRemoveUser()
        {
            await InsertUser();

            var result = await _repository.DeleteAsync(_user.Id);

            var count = await _context.Users.CountAsync();
            
            result.IsSuccess.Should().BeTrue();
            result.Success!.Should().BeEquivalentTo(RepositoryMessages.EntityRemoved.WithParameters(UserMessages.User));
            count.Should().Be(0);
        }

        [Fact]
        public async Task DeleteAsync_WhenUserNotExists_ShouldNotRemoveTheUser()
        {
            await InsertUser();

            var result = await _repository.DeleteAsync(1000);

            var count = await _context.Users.CountAsync();

            result.IsError.Should().BeTrue();
            result.Error!.Should().BeEquivalentTo(RepositoryMessages.EntityNotFound.WithParameters(UserMessages.User));

            count.Should().BeGreaterThanOrEqualTo(1);
        }

        [Fact]
        public async Task UpdateAsync_WhenUserExists_ShouldUpdateTheUser()
        {
            await InsertUser();

            var user = _user;

            user.Name = "Edited name";

            await _repository.UpdateAsync(user);

            var updatedUser = await _context.Users.FindAsync(user.Id);

            updatedUser!.Name.Should().Be(user.Name);
        }

        [Fact]
        public async Task UpdateAsync_WhenAnExceptionIsThrown_ShouldReturnFailure()
        {
            await InsertUser();

            var user = new User
            {
                Name = "Another Test",
                Email = "anothertest@test.com",
                Password = "AnotherPassword"
            };

            await InsertUser(user);

#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
            user.Email = null;
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.

            var result = await _repository.UpdateAsync(user);

            result.IsError.Should().BeTrue();
        }

        private async Task InsertUser(User? user = null)
        {
            await _context.Users.AddAsync(user ?? _user);
            await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
