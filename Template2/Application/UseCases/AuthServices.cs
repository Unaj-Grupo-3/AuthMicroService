using Application.Interfaces;
using Application.Models;
using Domain.Entities;


namespace Application.UseCases
{
    public class AuthServices : IAuthServices
    {
        private readonly IAuthCommands _commands;
        private readonly IAuthQueries _queries;
        private readonly IEncryptServices _encrypt;

        public AuthServices(IAuthCommands commands, IAuthQueries queries, IEncryptServices encrypt)
        {
            _commands = commands;
            _queries = queries;
            _encrypt = encrypt;
        }

        public async Task<AuthResponse2> CreateAuthentication(AuthReq req)
        {
            _encrypt.CreatePasswordHash(req.Password, out byte[] passwordHash, out byte[] passwordSalt);

            Authentication auth = new Authentication
            {
                Email = req.Email,
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt
            };

            await _commands.InsertAuthentication(auth);

            AuthResponse2 authResponse = new AuthResponse2
            {
                Email = req.Email
            };

            return authResponse;
        }

        public async Task<AuthResponse> GetAuthentication(AuthReq req)
        {
            var password = req.Password;
            var mail = req.Email;

            var auth = await _queries.GetAuthByEmail(mail);

            if (auth == null)
            {
                return null;
            }

            if (!_encrypt.VerifyPassword(password, auth.PasswordHash, auth.PasswordSalt))
            {
                return null;
            }

            AuthResponse response = new AuthResponse
            {
                Id = auth.AuthId,
                Email = auth.Email
            };

            return response;
        }
    }
}
