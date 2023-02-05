using Entities = GameAuth.Database.Models.Entities;
using GameAuth.Database.Repository.Interface;

using GameAuth.Api.Services.Interface;
using GameAuth.Email.Service.Interface;

namespace GameAuth.Api.Services;

public class ConfirmationEmailService : IConfirmationEmailService
{
    private readonly IEmailService emailService;
    private readonly IVerificationEmailRepository verificationEmailRepository;
    private readonly IEmailRepository emailRepository;

    public ConfirmationEmailService(
        IEmailService emailService,
        IVerificationEmailRepository verificationEmailRepository,
        IEmailRepository emailRepository)
    {
        this.emailService = emailService;
        this.verificationEmailRepository = verificationEmailRepository;
        this.emailRepository = emailRepository;
    }

    public Task<string> SendPasswordResetEmail(Entities.Account account)
    {
        throw new NotImplementedException();
    }

    public async Task<string> SendVerificationEmail(Entities.Account account, Entities.Email email)
    {
        var code = await verificationEmailRepository
            .UpsertNewVerificationEmail(account.Id, email.Id);

        var success = await emailService
            .SendVerificationEmail(email.Value, code);

        if (!success)
        {
            await verificationEmailRepository
                .DeleteVerificationEmailByEmailId(email.Id);
            throw new Exception("Failed to send verificationEmail and cleaned up after");
        }
        return code;
    }

    public async Task<Entities.Email?> VerifyEmail(long accountId, string code)
    {
        var verificationEmails = await verificationEmailRepository.GetVerificationEmailsByAccountId(accountId);

        var matchingVarificationEmails = verificationEmails.Where(e => e.Code.Equals(code));
        if (matchingVarificationEmails.Count() > 1) throw new InvalidDataException("There are two verification emails with identical code for the same account");

        var verificationEmail = matchingVarificationEmails.FirstOrDefault();
        if (verificationEmail is null) return default;

        if (!verificationEmail.Code.Equals(code)) return default;
        var verified = await emailRepository.VerifyEmail(verificationEmail.EmailId);

        return verified
            ? await emailRepository.GetEmail(verificationEmail.EmailId)
            : default;
    }
}