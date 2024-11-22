using Poupa.AI.Domain.Entities;
using Poupa.AI.Domain.Enums;

namespace Poupa.AI.Application.DTOs.Categories
{
    public class UpdateCategoryRequest(int id, int userId, string name, TransactionType type)
    {
        public int Id { get; } = id;
        public int UserId { get; } = userId;
        public string Name { get; } = name;
        public TransactionType Type { get; } = type;      
        
        public Category ToDomainEntity()
            => new() { Id = Id, UserId = UserId, Name = Name, Type = Type }; 
    }
}
