
using Application.Interfaces;
using Application.Models;
using Domain.Entities;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Application.UseCases
{
    public class TokenServices : ITokenServices
    {

        public string GenerateToken(Jwt jwt, AuthResponse auth, int userId)
        {
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, jwt.Subject),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString()),
                new Claim("UserId", userId.ToString()),
                new Claim("AuthId", auth.Id.ToString())
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwt.Key));

            var singIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature);

            var token = new JwtSecurityToken(
                    jwt.Issuer,
                    jwt.Audience,
                    claims,
                    expires: DateTime.Now.AddHours(1),
                    signingCredentials: singIn
                );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public bool ValidateToken(ClaimsIdentity identity, int userId)
        {
            try
            {
                var id = identity.Claims.FirstOrDefault(x => x.Type == "UserId").Value;

                if (id != userId.ToString())
                {
                    throw new ArgumentException();
                }

                return true;
            }

            catch (ArgumentException)
            {
                return false;
            }
        }
    }
}
