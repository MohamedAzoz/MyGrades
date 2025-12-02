namespace MyGrades.Application.Contracts.Services
{
    public interface IEmailService
    {
        Task SendEmailVerificationAsync(string toEmail, string subject, string htmlBody);
    }
}
