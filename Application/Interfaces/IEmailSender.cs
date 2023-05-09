
namespace Application.Interfaces
{
    public interface IEmailSender
    {
        Task SendEmail(string subject, string toEmail, string userName, string message);
    }
}
