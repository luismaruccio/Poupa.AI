using Bogus;
using Poupa.AI.Domain.Entities;
using Poupa.AI.Domain.Enums;

namespace Poupa.AI.Infra.Tests.DataGenerators
{
    public class CategoryGenerator
    {
        private readonly Faker<Category> _categoryFaker;

        public CategoryGenerator()
        {
            _categoryFaker = new Faker<Category>()
                .RuleFor(u => u.Name, f => f.Commerce.Categories(1).First())
                .RuleFor(u => u.Type, f => f.Random.Enum<TransactionType>());
        }

        public Category GetFakeCategory()
            => _categoryFaker.Generate();
    }
}
