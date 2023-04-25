using Application.Interfaces;
using Application.Models;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;
using System.Diagnostics.CodeAnalysis;

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
                var validation = _validations.Validate(req).Result;

                if (validation.ElementAt(0).Key)
                {
                    var response = await _services.CreateAuthentication(req);
                    return new JsonResult(new { Message = "Exito.", Response = response }) { StatusCode = 201 };
                }
                else
                {
                    var errores = validation.ElementAt(0).Value;
                    return new JsonResult(new { Message = "Existen errores en la petición.", Response = errores }) { StatusCode = 400 };
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

                bool postIsValid = await _userApiServices.GetUserByAuthId(auth.Id);

                if (!postIsValid)
                {
                    return new JsonResult(new { Message = _userApiServices.GetMessage(), Response = _userApiServices.GetResponse() }) { StatusCode = _userApiServices.GetStatusCode() };
                }

                int userId = int.Parse(_userApiServices.GetResponse().RootElement.GetProperty("userId").ToString());

                var token = _tokenServices.GenerateToken(jwt, auth, userId);

                return new JsonResult(new { Message = "Ha iniciado sesión", Token = token }) { StatusCode = 200};

            }
            catch (Exception)
            {
                return new JsonResult(new { Message = "Problema interno del servidor." }) { StatusCode = 500 };
            }

        }
    }
}
