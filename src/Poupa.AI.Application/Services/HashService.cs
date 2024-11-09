using Poupa.AI.Application.Interfaces.Services;
using Sodium;

namespace Poupa.AI.Application.Services
{
    public class HashService : IHashService
    {
        public string HashPassword(string password)
            => PasswordHash.ArgonHashString(password, PasswordHash.StrengthArgon.Moderate);

        public bool VerifyPassword(string password, string hashedPassword)
            => PasswordHash.ArgonHashStringVerify(hashedPassword, password);
    }
}
