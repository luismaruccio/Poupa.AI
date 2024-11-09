using Poupa.AI.Domain.Entities;

namespace Poupa.AI.Application.DTOs.Users
{
    public class CreateUserResponse(int id, string name, string email)
    {
        public int Id { get; set; } = id;
        public string Name { get; set; } = name;
        public string Email { get; set; } = email;

        public static CreateUserResponse FromUser(User user)
            => new(user.Id, user.Name, user.Email);
    }
}
