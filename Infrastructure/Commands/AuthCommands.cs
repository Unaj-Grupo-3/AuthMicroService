using Application.Interfaces;
using Application.Models;
using Domain.Entities;
using Infrastructure.Persistance;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Commands
{
    public class AuthCommands : IAuthCommands
    {
        private readonly ExpresoDbContext _context;

        public AuthCommands(ExpresoDbContext context)
        {
            _context = context;
        }

        public async Task<Authentication> InsertAuthentication(Authentication auth)
        {
            _context.Add(auth);

            await _context.SaveChangesAsync();

            return auth;
        }

        public async Task<Authentication> AlterAuth(Guid authId, byte[] hash, byte[] salt)
        {
            var auth = await _context.Authentications.FirstOrDefaultAsync(e => e.AuthId.Equals(authId));

            auth.PasswordHash = hash;
            auth.PasswordSalt = salt;

            await _context.SaveChangesAsync();

            return auth;
        }
    }
}
