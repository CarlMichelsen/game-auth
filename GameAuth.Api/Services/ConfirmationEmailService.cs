using GameAuth.Database.Models.Entities;
using GameAuth.Database.Repository.Interface;

using GameAuth.Api.Services.Interface;
using GameAuth.Email.Service.Interface;

namespace GameAuth.Api.Services;

public class ConfirmationEmailService : IConfirmationEmailService
{
    private readonly IEmailService emailService;
    private readonly IVerificationEmailRepository verificationEmailRepository;
    private readonly IAccountRepository accountRepository;

    public ConfirmationEmailService(
        IEmailService emailService,
        IVerificationEmailRepository verificationEmailRepository,
        IAccountRepository accountRepository)
    {
        this.emailService = emailService;
        this.verificationEmailRepository = verificationEmailRepository;
        this.accountRepository = accountRepository;
    }

    public async Task<string> SendVerificationEmail(Account account)
    {
        var code = await verificationEmailRepository
            .UpsertNewVerificationEmail(account.Id);
        var email = account.Emails.Single(e => e.IsPrimary && e.AccountId.Equals(account.Id));
        var success = await emailService
            .SendVerificationEmail(email.Value, code);

        if (!success)
        {
            await verificationEmailRepository
                .DeleteVerificationEmailByAccountId(account.Id);
            throw new Exception("Failed to send verificationEmail and cleaned up after");
        }
        return code;
    }

    public async Task<bool> VerifyEmail(long accountId, string code)
    {
        var verificationEmail = await verificationEmailRepository.GetVerificationEmailByAccountId(accountId);
        if (verificationEmail is null) return false;
        if (!verificationEmail.Code.Equals(code)) return false;
        return await accountRepository.VerifyEmail(accountId);
    }

    public Task<string> SendPasswordResetEmail(Account account)
    {
        throw new NotImplementedException();
    }
}