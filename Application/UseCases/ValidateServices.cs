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

        public async Task<IDictionary<bool, IDictionary<string, string>>> Validate(AuthReq authReq)
        {
            bool password = await ValidateLenght(authReq.Password, "Password", 32, 8);
            var mail = await VerifyMail(authReq.Email);

            
            IDictionary<bool, IDictionary<string, string>> newDictionary = new Dictionary<bool, IDictionary<string, string>>() { };

            if (password && mail)
            {
                newDictionary.Add(true, errors);
                return newDictionary;
            }
            else
            {
                newDictionary.Add(false, errors);
                return newDictionary;
            }
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
