using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Poupa.AI.Infra.Data;

namespace Poupa.AI.Infra.Tests.InMemoryDB
{
    internal class InMemoryDBContextFactory
    {
        private readonly PoupaAIDbContext _context;

        public InMemoryDBContextFactory()
        {
            var _connection = new SqliteConnection("Filename=:memory:");
            _connection.Open();

            var options = new DbContextOptionsBuilder<PoupaAIDbContext>()
                .UseSqlite(_connection)
                .Options;

            _context = new PoupaAIDbContext(options);

            _context.Database.EnsureCreated();
            _context.Database.GetAppliedMigrationsAsync().Wait();

        }

        public PoupaAIDbContext GetContext()
            => _context;
    }
}
