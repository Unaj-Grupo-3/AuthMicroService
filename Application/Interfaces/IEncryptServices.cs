

namespace Application.Interfaces
{
    public interface IEncryptServices
    {
        void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt);
        bool VerifyPassword(string password, byte[] passwordHash, byte[] passwordSalt);
    }
}
