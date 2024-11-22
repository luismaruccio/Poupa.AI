using Poupa.AI.Domain.Entities;
using Poupa.AI.Domain.Enums;

namespace Poupa.AI.Application.DTOs.Categories
{
    public class CreateCategoryRequest(string name, TransactionType type, int userId)
    {
        public string Name { get; } = name;
        public TransactionType Type { get; } = type;
        public int UserId { get; } = userId;

        public Category ToDomainEntity()
            => new() { UserId = UserId, Name = Name, Type = Type };
        
    }
}
