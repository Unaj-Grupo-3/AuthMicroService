using Application.Models;


namespace Application.Interfaces
{
    public interface IValidateServices
    {
        Task<IDictionary<bool, IDictionary<string, string>>> Validate(AuthReq authReq);
        Task<bool> ValidateLenght(string verify, string tag, int maxLenght);
        Task<bool> ValidateLenght(string verify, string tag, int maxLenght, int minLenght);
        Task<bool> ValidateCharacters(string verify, string tag);
        Task<bool> VerifyMail(string mail);
    }
}
