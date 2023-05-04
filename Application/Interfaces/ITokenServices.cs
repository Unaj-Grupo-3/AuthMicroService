using Application.Models;
using Domain.Entities;
using System.Security.Claims;

namespace Application.Interfaces
{
    public interface ITokenServices
    {

        bool ValidateToken(ClaimsIdentity identity, Guid userId);

        string GenerateToken(Jwt jwt, AuthResponse auth, Guid userId);
    }
}
