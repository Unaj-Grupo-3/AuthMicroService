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
