using GameAuth.Database.Models.Entities;
using GameAuth.Database.Repository.Interface;

using GameAuth.Api.Services.Interface;
using GameAuth.Email.Service.Interface;
using Microsoft.EntityFrameworkCore;

namespace GameAuth.Api.Services;

public class ConfirmationEmailService : IConfirmationEmailService
{
    private readonly IEmailService emailService;
    private readonly IVerificationEmailRepository verificationEmailRepository;

    public ConfirmationEmailService(
        IEmailService emailService,
        IVerificationEmailRepository verificationEmailRepository)
    {
        this.emailService = emailService;
        this.verificationEmailRepository = verificationEmailRepository;
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

    public Task<string> SendPasswordResetEmail(Account account)
    {
        throw new NotImplementedException();
    }
}