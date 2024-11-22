using Poupa.AI.Domain.Entities;
using Poupa.AI.Domain.Enums;

namespace Poupa.AI.Application.DTOs.Categories
{
    public class CategoryResponse(int id, int userId, string name, TransactionType type)
    {
        public int Id { get; } = id;
        public int UserId { get; } = userId;
        public string Name { get; } = name;
        public TransactionType Type { get; } = type;

        public static CategoryResponse FromDomainEntity(Category category)
            => new(category.Id, category.UserId, category.Name, category.Type);

        public static IReadOnlyList<CategoryResponse> FromDomainEntities(IReadOnlyList<Category> categories)
         => categories.Select(c => new CategoryResponse(c.Id, c.UserId, c.Name, c.Type)).ToList();

    }
}
