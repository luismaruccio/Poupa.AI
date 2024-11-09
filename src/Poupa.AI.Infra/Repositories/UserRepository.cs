using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Poupa.AI.Common.Extensions.Messages;
using Poupa.AI.Common.Utils;
using Poupa.AI.Domain.Entities;
using Poupa.AI.Domain.Interfaces.Repositories;
using Poupa.AI.Domain.Messages.Common;
using Poupa.AI.Domain.Messages.Users;
using Poupa.AI.Infra.Data;

namespace Poupa.AI.Infra.Repositories
{
    public class UserRepository(PoupaAIDbContext context, ILogger<UserRepository> logger) : IUserRepository
    {
        private readonly PoupaAIDbContext _context = context;
        private readonly ILogger<UserRepository> _logger = logger;

        public async Task<Either<string, User>> AddAsync(User entity)
        {
            try
            {
                _logger.LogInformation("AddAsync - Received {entity}", entity);
                await _context.Users.AddAsync(entity);
                await _context.SaveChangesAsync();
                _logger.LogInformation("AddAsync - Added user with success {entity}", entity);
                return Either<string, User>.FromSuccess(entity);
            }
            catch (Exception ex)
            {
                _logger.LogError("AddAsync - Failure on add the user {entity}. Error: {error}", entity, ex);
                return Either<string, User>.FromError(ex.Message);
            }
        }

        public async Task<Either<string, string>> DeleteAsync(int id)
        {
            var userToBeDeleted = await GetByIdAsync(id);
            if (userToBeDeleted.IsError)
            {
                return Either<string, string>.FromError(userToBeDeleted.Error!);
            }
            else
            {
                _context.Users.Remove(userToBeDeleted.Success!);
                await _context.SaveChangesAsync();
                return Either<string, string>.FromSuccess(RepositoryMessages.EntityRemoved.WithParameters(UserMessages.User));
            }
        }

        public async Task<Either<string, User>> GetByEmailAsync(string email)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
            if (user is null)
            {
                return Either<string, User>.FromError(RepositoryMessages.EntityNotFoundBy.WithParameters([UserMessages.User, UserMessages.Email]));
            }
            return Either<string, User>.FromSuccess(user);
        }

        public async Task<Either<string, User>> GetByIdAsync(int id)
        {
            var user = await _context.Users.FindAsync(id);

            if (user is null)
            {
                return Either<string, User>.FromError(RepositoryMessages.EntityNotFound.WithParameters(UserMessages.User));
            }

            return Either<string, User>.FromSuccess(user);
        }

        public async Task<Either<string, User>> UpdateAsync(User entity)
        {
            try
            {
                _logger.LogInformation("UpdateAsync - Received {entity}", entity);
                _context.Users.Update(entity);
                await _context.SaveChangesAsync();
                _logger.LogInformation("UpdateAsync - Updated user with success {entity}", entity);
                return Either<string, User>.FromSuccess(entity);
            }
            catch (Exception ex)
            {
                _logger.LogError("UpdateAsync - Failure on update the user {entity}. Error: {error}", entity, ex);
                return Either<string, User>.FromError(ex.Message);
            }
        }
    }
}
