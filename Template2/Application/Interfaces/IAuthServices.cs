using Application.Models;


namespace Application.Interfaces
{
    public interface IAuthServices
    {
        Task<AuthResponse2> CreateAuthentication(AuthReq req);
        Task<AuthResponse> GetAuthentication(AuthReq req);
    }
}
