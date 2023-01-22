namespace GameAuth.Email.Service.Interface;

public interface IEmailService
{
    Task<bool> SendVerificationEmail(string destinationEmail, string code);
}