namespace GameAuth.Email.Service.Interface;

public interface IEmailService
{
    Task SendSupportEmail(string destinationEmail, string subject, string text);
}