using Application.Interfaces;
using Application.Models;
using System.Text.RegularExpressions;


namespace Application.UseCases
{
    public class ValidateServices : IValidateServices
    {
        private readonly IAuthQueries _authQueries;
        public Dictionary<string, string> errors { get; set; }

        public ValidateServices(IAuthQueries authQueries)
        {
            _authQueries = authQueries;
            errors = new Dictionary<string, string>();
        }

        public async Task<IDictionary<string, string>> Validate(AuthReq authReq)
        {
            var password = await CheckPassword(authReq.Password);
            bool mail = await VerifyMail(authReq.Email);

            return errors;
        }

        public async Task<IDictionary<string, string>> CheckPassword(string passwd)
        {
            bool len = await ValidateLenght(passwd, "Password", 32, 8);
            Match matchNumeros = Regex.Match(passwd, @"\d");
            Match matchMayusculas = Regex.Match(passwd, @"[A-Z]");
            Match matchMinusculas = Regex.Match(passwd, @"[a-z]");
            Match matchEspeciales = Regex.Match(passwd, @"[ñÑ\-_¿.#¡/()*,.;:@]");

            if (!matchNumeros.Success || !matchMayusculas.Success || !matchMinusculas.Success || !matchEspeciales.Success)
            {
                errors.Add("passwd", "La contraseña debera contener por lo menos un número, una mayuscula, una minuscula y un caracter especial ej: ñÑ\\-_¿.#¡/()*,.;:@.");
            }

            return errors;
        }
        public async Task<bool> ValidateLenght(string verify, string tag, int maxLenght)
        {
            if (verify.Length > maxLenght)
            {
                errors.Add(tag, "La cadena ingresada en uno de los campos es muy larga. Máximos caracteres permitidos: " + maxLenght);
                return false;
            }
            if (verify.Trim() == "")
            {
                errors.Add(tag, "No se ingresó ningún dato en uno de los campos.");
                return false;
            }

            return true;
        }

        public async Task<bool> ValidateLenght(string verify, string tag, int maxLenght, int minLenght)
        {
            if (verify.Length > maxLenght)
            {
                errors.Add(tag, "La cadena ingresada en uno de los campos es muy larga. Máximos caracteres permitidos: " + maxLenght);
                return false;
            }
            if (verify.Length < minLenght)
            {
                errors.Add(tag, "La cadena es muy corta. Minimo de carateres: " + minLenght );
                return false;
            }
            if (verify.Trim() == "")
            {
                errors.Add(tag, "No se ingresó ningún dato en uno de los campos.");
                return false;
            }

            return true;
        }

        public async Task<bool> ValidateCharacters(string verify, string tag)
        {
            if (!Regex.IsMatch(verify, @"^[A-Za-zÀ-ÿ_@.0-9&\s]+$"))
            {
                errors.Add(tag, "Caracteres prohibidos.");
                return false;
            }
            else
            {
                return true;
            }
        }

        public async Task<bool> VerifyMail(string mail)
        {
            if(mail.Contains('@') && await ValidateCharacters(mail, "Mail"))
            {
                var result = await _authQueries.GetAuthByEmail(mail);

                if (result == null)
                {
                    return true;

                }
                else
                {
                    errors.Add("Mail2", "El mail ingresado ya fue registrado anteriormente.");
                    return false;
                }
            }
            else
            {
                errors.Add("Mail2", "Revise el mail ingresado, puede contener caracteres prohibidos o no es un mail válido.");
                return false;
            }

        }
    }
}
