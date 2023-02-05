using Entities = GameAuth.Database.Models.Entities;

namespace GameAuth.Api.Services.Interface;

public interface IConfirmationEmailService
{
    Task<string> SendVerificationEmail(Entities.Account account, Entities.Email email);
    Task<Entities.Email?> VerifyEmail(long accountId, string code); // returns email that was verified
    Task<string> SendPasswordResetEmail(Entities.Account account);
}