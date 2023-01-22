using GameAuth.Database.Models.Entities;

namespace GameAuth.Api.Services.Interface;

public interface IConfirmationEmailService
{
    Task<string> SendVerificationEmail(Account account);
    Task<string> SendPasswordResetEmail(Account account);
}