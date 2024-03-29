﻿using Application.Interfaces;
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

        public async Task<AuthResponse> CreateAuthentication(AuthReq req)
        {
            _encrypt.CreatePasswordHash(req.Password, out byte[] passwordHash, out byte[] passwordSalt);

            Authentication auth = new Authentication
            {
                Email = req.Email,
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt
            };

            Authentication create = await _commands.InsertAuthentication(auth);

            AuthResponse authResponse = new AuthResponse
            {
                Id = create.AuthId,
                Email = req.Email,
                UserId = create.UserId,

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
                Email = auth.Email,
                UserId = auth.UserId
            };

            return response;
        }

        public async Task<AuthResponse> GetMail(Guid authId)
        {
            var response = await _queries.SelectMailByAuthId(authId);

            return response;
        }

        public async Task<AuthResponse> ChangePassword(Guid authId, ChangePassReq req)
        {
            _encrypt.CreatePasswordHash(req.Password, out byte[] passwordHash, out byte[] passwordSalt);

            var query = await _commands.AlterAuth(authId, passwordHash, passwordSalt);

            if (query != null && query.PasswordHash == passwordHash && query.PasswordSalt == passwordSalt)
            {
                AuthResponse resp = new AuthResponse
                {
                    Id = query.AuthId,
                    Email = query.Email
                };

                return resp;
            }
            else
            {
                return null;
            }
        }
    }
}
