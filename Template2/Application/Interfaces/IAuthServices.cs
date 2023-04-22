using Application.Models;


namespace Application.Interfaces
{
    public interface IAuthServices
    {
        Task<AuthResponse> CreateAuthentication(AuthReq req);
        Task<AuthResponse> GetAuthentication(AuthReq req);
    }
}
