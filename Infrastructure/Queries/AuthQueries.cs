using Application.Interfaces;
using Application.Models;
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

        public async Task<AuthResponse> SelectMailByAuthId(Guid authId)
        {
            var response = await _context.Authentications
                                            .Select(e => new AuthResponse
                                            {
                                                Id = e.AuthId,
                                                Email = e.Email,
                                                UserId = e.UserId
                                            }).FirstOrDefaultAsync(x => x.Id == authId);

            return response;
        }
    }
}
