using Application.Interfaces;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace Application.UseCases
{
    public class EmailSender:IEmailSender
    {
        public async Task SendEmail(string subject, string toEmail, string userName, string message)
        {
            var apiKey = "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";
            var client = new SendGridClient(apiKey);
            var from = new EmailAddress("expressodelasdiez@gmail.com", "ExpressoDeLasDiez");
            var to = new EmailAddress(toEmail, userName);
            var plainTextContent = message;
            var htmlContent = "";
            var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);
            var response = await client.SendEmailAsync(msg);
        }
    }
}
