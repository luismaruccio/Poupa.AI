using Poupa.AI.Domain.Entities.Common;
using Poupa.AI.Domain.Enums;

namespace Poupa.AI.Domain.Entities
{
    public class Category : Entity
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public required string Name { get; set; }
        public TransactionType Type { get; set; }
        public bool IsDeleted { get; set; }

        public User? User { get; set; }
    }
}
