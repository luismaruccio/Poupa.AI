using Microsoft.EntityFrameworkCore;
using Poupa.AI.Domain.Entities;

namespace Poupa.AI.Infra.Data
{
    public class PoupaAIDbContext(DbContextOptions<PoupaAIDbContext> options) : DbContext(options)
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Category> Categories { get; set; }
    }
}
