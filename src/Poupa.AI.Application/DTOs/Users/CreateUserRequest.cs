using Poupa.AI.Domain.Entities;

namespace Poupa.AI.Application.DTOs.Users
{
    public class CreateUserRequest(string name, string email, string password)
    {
        public string Name { get; } = name;
        public string Email { get; } = email;
        public string Password { get; } = password;

        public User ToDomainEntity()
            => new() { Name = Name, Email = Email, Password = Password };
    }
}
