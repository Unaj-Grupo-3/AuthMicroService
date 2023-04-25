using Domain.Entities;


namespace Application.Interfaces
{
    public interface IAuthQueries
    {
        Task<Authentication> GetAuthByEmail(string mail);
    }
}
