using Domain.Entities;


namespace Application.Interfaces
{
    public interface IAuthCommands
    {
        Task InsertAuthentication(Authentication auth);
    }
}
