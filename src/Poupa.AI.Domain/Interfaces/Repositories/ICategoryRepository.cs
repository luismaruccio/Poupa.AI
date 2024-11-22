using Poupa.AI.Common.Utils;
using Poupa.AI.Domain.Entities;
using Poupa.AI.Domain.Enums;
using Poupa.AI.Domain.Interfaces.Repositories.Common;

namespace Poupa.AI.Domain.Interfaces.Repositories
{
    public interface ICategoryRepository : IRepository<Category>
    {
        public Task<Either<string, IReadOnlyList<Category>>> GetAllByUserAsync(int userId);
        public Task<Either<string, IReadOnlyList<Category>>> GetAllByUserAndTypeAsync(int userId, TransactionType type);
    }
}
