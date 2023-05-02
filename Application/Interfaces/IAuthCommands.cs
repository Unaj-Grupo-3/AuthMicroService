using Domain.Entities;


namespace Application.Interfaces
{
    public interface IAuthCommands
    {
        Task<Authentication> InsertAuthentication(Authentication auth);
        Task<Authentication> AlterAuth(Guid authId, byte[] hash, byte[] salt);
    }
}
