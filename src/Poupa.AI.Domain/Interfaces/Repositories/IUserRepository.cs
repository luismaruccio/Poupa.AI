using Poupa.AI.Common.Utils;
using Poupa.AI.Domain.Entities;
using Poupa.AI.Domain.Interfaces.Repositories.Common;

namespace Poupa.AI.Domain.Interfaces.Repositories
{
    public interface IUserRepository : IRepository<User>
    {
        public Task<Either<string, User>> GetByEmailAsync(string email);
    }
}
