using Poupa.AI.Application.DTOs.Categories;
using Poupa.AI.Application.DTOs.Common;
using Poupa.AI.Common.Utils;
using Poupa.AI.Domain.Enums;

namespace Poupa.AI.Application.Interfaces.Services
{
    public interface ICategoryService
    {
        public Task<Either<MessageResponse, CategoryResponse>> CreateCategoryAsync(CreateCategoryRequest request);
        public Task<Either<MessageResponse, CategoryResponse>> UpdateCategoryAsync(UpdateCategoryRequest request);
        public Task<Either<MessageResponse, MessageResponse>> DeleteCategoryAsync(int id);
        public Task<Either<MessageResponse, CategoryResponse>> GetCategoryByIdAsync(int id);
        public Task<Either<MessageResponse, CategoryBreakdownResponse>> GetCategoriesByUserAsync(int userId);
        public Task<Either<MessageResponse, IReadOnlyList<CategoryResponse>>> GetCategoriesByUserAndTypeAsync(int userId, TransactionType type);
    }
}
