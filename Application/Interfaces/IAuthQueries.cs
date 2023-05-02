using Application.Models;
using Domain.Entities;


namespace Application.Interfaces
{
    public interface IAuthQueries
    {
        Task<Authentication> GetAuthByEmail(string mail);
        Task<AuthResponse> SelectMailByAuthId(Guid authId);
    }
}
