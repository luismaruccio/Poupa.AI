using Poupa.AI.Domain.Entities;

namespace Poupa.AI.Application.DTOs.Users
{
    public class CreateUserResponse(int id, string name, string email)
    {
        public int Id { get; } = id;
        public string Name { get; } = name;
        public string Email { get; } = email;

        public static CreateUserResponse FromUser(User user)
            => new(user.Id, user.Name, user.Email);
    }
}
