using MyGrades.Application.Contracts.Services;
using System.Net;
using System.Net.Mail;

namespace MyGrades.Application.Services
{
    public class EmailService : IEmailService
    {
        public async Task SendEmailVerificationAsync(string toEmail, string subject, string htmlBody)
        {
            var smtpclient = new SmtpClient("smtp.gmail.com")
            {
                Port = 587,
                Credentials = new NetworkCredential("mygrades066@gmail.com", "owmj ubst vvcf blnj"),
                EnableSsl = true
            };
            var mailMessage = new MailMessage()
            {
                From = new MailAddress("mygrades066@gmail.com", "My Grades"),
                Subject = subject,
                Body = htmlBody,
                IsBodyHtml = true
            };

            mailMessage.To.Add(toEmail);
            await smtpclient.SendMailAsync(mailMessage);
        }

    }
}

