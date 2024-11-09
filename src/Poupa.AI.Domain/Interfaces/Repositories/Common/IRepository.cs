using Poupa.AI.Common.Utils;
using Poupa.AI.Domain.Entities.Common;

namespace Poupa.AI.Domain.Interfaces.Repositories.Common
{
    public interface IRepository<T> where T : Entity
    {
        public Task<Either<string,T>> GetByIdAsync(int id);
        public Task<Either<string,T>> AddAsync(T entity);
        public Task<Either<string, T>> UpdateAsync(T entity);
        public Task<Either<string, string>> DeleteAsync(int id);
    }
}
