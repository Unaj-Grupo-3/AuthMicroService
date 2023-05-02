using Application.Models;


namespace Application.Interfaces
{
    public interface IAuthServices
    {
        Task<AuthResponse> CreateAuthentication(AuthReq req);
        Task<AuthResponse> GetAuthentication(AuthReq req);
        Task<AuthResponse> GetMail(Guid authId);
        Task<AuthResponse> ChangePassword(Guid authId, ChangePassReq req);
    }
}
