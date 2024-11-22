using Poupa.AI.Application.DTOs.Common;
using Poupa.AI.Application.DTOs.Users;
using Poupa.AI.Common.Utils;

namespace Poupa.AI.Application.Interfaces.Services
{
    public interface IUserService
    {
        public Task<Either<MessageResponse, CreateUserResponse>> CreateUserAsync(CreateUserRequest request);
    }
}
