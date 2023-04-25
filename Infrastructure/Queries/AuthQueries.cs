using Application.Interfaces;
using Domain.Entities;
using Infrastructure.Persistance;
using Microsoft.EntityFrameworkCore;


namespace Infrastructure.Queries
{
    public class AuthQueries : IAuthQueries
    {
        private readonly ExpresoDbContext _context;

        public AuthQueries(ExpresoDbContext context)
        {
            _context = context;
        }

        public async Task<Authentication> GetAuthByEmail(string mail)
        {
            var auth = await _context.Authentications
                                .FirstOrDefaultAsync(e => e.Email == mail);

            return auth;
        }
    }
}
