using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Poupa.AI.Common.Extensions.Messages;
using Poupa.AI.Common.Utils;
using Poupa.AI.Domain.Entities;
using Poupa.AI.Domain.Enums;
using Poupa.AI.Domain.Interfaces.Repositories;
using Poupa.AI.Domain.Messages;
using Poupa.AI.Domain.Messages.Common;
using Poupa.AI.Infra.Data;

namespace Poupa.AI.Infra.Repositories
{
    public class CategoryRepository(PoupaAIDbContext context, ILogger<CategoryRepository> logger) : ICategoryRepository
    {
        private readonly PoupaAIDbContext _context = context;
        private readonly ILogger<CategoryRepository> _logger = logger;

        public async Task<Either<string, Category>> AddAsync(Category entity)
        {
            try
            {
                _logger.LogInformation("AddAsync - Received {entity}", entity);
                await _context.Categories.AddAsync(entity);
                await _context.SaveChangesAsync();
                _logger.LogInformation("AddAsync - Added the category with success {entity}", entity);
                return Either<string, Category>.FromSuccess(entity);
            }
            catch (Exception ex)
            {
                _logger.LogError("AddAsync - Failure on add the category {entity}. Error: {error}", entity, ex);
                return Either<string, Category>.FromError(ex.Message);
            }
        }

        public async Task<Either<string, string>> DeleteAsync(int id)
        {
            var result = await GetByIdAsync(id);
            if (result.IsError)
            {
                return Either<string, string>.FromError(result.Error!);
            }

            var category = result.Success!;
            category.IsDeleted = true;

            try
            {
                _context.Categories.Update(category);
                await _context.SaveChangesAsync();
                return Either<string, string>.FromSuccess(RepositoryMessages.EntityRemoved.WithParameters(CategoryMessages.Category));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "DeleteAsync - Failure to soft delete category with id {id}", id);
                return Either<string, string>.FromError(ex.Message);
            }
        }

        public async Task<Either<string, IReadOnlyList<Category>>> GetAllByUserAsync(int userId)
        {
            var categories = await _context.Categories
                .Where(c => c.UserId == userId && c.IsDeleted == false)
                .ToListAsync();

            if (categories.Count == 0)
                return Either<string, IReadOnlyList<Category>>.FromError(RepositoryMessages.EntityNotFoundBy.WithParameters([CategoryMessages.Categories, CategoryMessages.User]));

            return Either<string, IReadOnlyList<Category>>.FromSuccess(categories);

        }

        public async Task<Either<string, IReadOnlyList<Category>>> GetAllByUserAndTypeAsync(int userId, TransactionType type)
        {
            var categories = await _context.Categories
                .Where(c => c.UserId == userId && c.Type == type && c.IsDeleted == false)
                .ToListAsync();

            if (categories.Count == 0)
                return Either<string, IReadOnlyList<Category>>.FromError(RepositoryMessages.EntityNotFoundBy.WithParameters([CategoryMessages.Categories, CategoryMessages.User]));

            return Either<string, IReadOnlyList<Category>>.FromSuccess(categories);
        }

        public async Task<Either<string, Category>> GetByIdAsync(int id)
        {
            var category = await _context.Categories.FindAsync(id);

            if (category is null)
            {
                return Either<string, Category>.FromError(RepositoryMessages.EntityNotFound.WithParameters(CategoryMessages.Category));
            }

            return Either<string, Category>.FromSuccess(category);
        }

        public async Task<Either<string, Category>> UpdateAsync(Category entity)
        {
            try
            {
                _logger.LogInformation("UpdateAsync - Received {entity}", entity);
                _context.Categories.Update(entity);
                await _context.SaveChangesAsync();
                _logger.LogInformation("UpdateAsync - Updated category with success {entity}", entity);
                return Either<string, Category>.FromSuccess(entity);
            }
            catch (Exception ex)
            {
                _logger.LogError("UpdateAsync - Failure on update the category {entity}. Error: {error}", entity, ex);
                return Either<string, Category>.FromError(ex.Message);
            }
        }
    }
}
