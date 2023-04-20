using Application.Interfaces;
using Application.Models;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthServices _services;
        private readonly IValidateServices _validations;

        public AuthController(IAuthServices services, IValidateServices validations)
        {
            _services = services;
            _validations = validations;
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
            catch (AggregateException)
            {
                return new JsonResult(new { Message = "Problema interno del servidor." }) { StatusCode = 500 };
            }
            
        }
    }
}
