using Application.Interfaces;
using Application.Models;
using Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;
using System.Diagnostics.CodeAnalysis;
using System.Security.Claims;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Model;

namespace Presentation.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthServices _services;
        private readonly IValidateServices _validations;
        private readonly ITokenServices _tokenServices;
        private readonly IConfiguration _configuration;
        private readonly IUserApiServices _userApiServices;

        public AuthController(IUserApiServices userApiServices,IAuthServices services, IValidateServices validations, ITokenServices tokenServices, IConfiguration configuration)
        {
            _services = services;
            _validations = validations;
            _tokenServices = tokenServices;
            _configuration = configuration;
            _userApiServices = userApiServices;
            
        }

        [HttpPost]
        public async Task<IActionResult> CreateAuth(AuthReq req)
        {
            try
            {
                var errors = _validations.Validate(req).Result;

                if (errors.Count > 0)
                {
                    return new JsonResult(new { Message = "Existen errores en la petición.", Response = errors }) { StatusCode = 400 };
                }
                else
                {
                    var response = await _services.CreateAuthentication(req);
                    return new JsonResult(new { Message = "Exito.", Response = response }) { StatusCode = 201 };
                }
            }
            catch (Exception)
            {
                return new JsonResult(new { Message = "Problema interno del servidor." }) { StatusCode = 500 };
            }
            
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(AuthReq req)
        {
            try
            {
                AuthResponse auth = await _services.GetAuthentication(req);

                if (auth == null)
                {
                    return new JsonResult(new { Message = "Credenciales Incorrectas" }) { StatusCode = 400 };
                }

                var jwt = _configuration.GetSection("Jwt").Get<Jwt>();

                var token = _tokenServices.GenerateToken(jwt, auth);

                return new JsonResult(new { Message = "Ha iniciado sesión", Token = token }) { StatusCode = 200};

            }
            catch (Exception)
            {
                return new JsonResult(new { Message = "Problema interno del servidor." }) { StatusCode = 500 };
            }
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetMail()
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            var authId = Guid.Parse(identity.Claims.FirstOrDefault(x => x.Type == "AuthId").Value);

            var response = await _services.GetMail(authId);

            return new JsonResult(response);
        }

        [HttpPut]
        [Authorize]
        public async Task<IActionResult> ChangePassword(ChangePassReq req)
        {
            var errors = await _validations.CheckPassword(req.Password);

            if (errors.Count > 0)
            {
                return new JsonResult(new { Message = "Existen errores en la petición", Response = errors }) { StatusCode = 400 };
            }

            var identity = HttpContext.User.Identity as ClaimsIdentity;
            var authId = Guid.Parse(identity.Claims.FirstOrDefault(x => x.Type == "AuthId").Value);

            var response = await _services.ChangePassword(authId, req);

            return new JsonResult(response);
        }
    }
}
