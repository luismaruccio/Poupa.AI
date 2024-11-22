using Bogus;
using Poupa.AI.Domain.Entities;

namespace Poupa.AI.Infra.Tests.DataGenerators
{
    public class UserGenerator
    {
        private readonly Faker<User> _userFaker;

        public UserGenerator()
        {
            _userFaker = new Faker<User>()
                .RuleFor(u => u.Name, f => f.Name.FullName())
                .RuleFor(u => u.Email, f => f.Internet.Email())
                .RuleFor(u => u.Password, f => f.Internet.Password());
        }

        public User GetFakeUser()
            => _userFaker.Generate();
    }
}
