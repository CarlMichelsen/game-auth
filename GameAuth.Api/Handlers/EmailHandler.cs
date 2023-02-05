using System.Security.Claims;
using GameAuth.Api.Models.Dto;
using GameAuth.Api.Handlers.Interface;
using GameAuth.Api.Services.Interface;
using GameAuth.Database.Repository.Interface;
using Entities = GameAuth.Database.Models.Entities;

namespace GameAuth.Api.Handlers;

public class EmailHandler : IEmailHandler
{
    private readonly IConfirmationEmailService confirmationEmailService;
    private readonly IAccountRepository accountRepository;

    public EmailHandler(
        IConfirmationEmailService confirmationEmailService,
        IAccountRepository accountRepository)
    {
        this.confirmationEmailService = confirmationEmailService;
        this.accountRepository = accountRepository;
    }

    public async Task<AuthResponse<string>> ResendVerificationEmail(ClaimsIdentity identity, string email)
    {
        var res = new AuthResponse<string>();
        var account = await GetAccountFromClaims(identity);
        if (account is null)
        {
            res.Errors.Add("failed to find account");
            return res;
        }

        var allEmails = GetAllEmailsFromAccount(account);

        var foundEmails = allEmails.Where(e => e.Value.Equals(email));
        if (foundEmails.Count() > 1) throw new InvalidDataException($"There are multiple identical emails for the same account <{account.Id}>");

        if (!foundEmails.Any())
        {
            res.Errors.Add("failed to find email");
            return res;
        }

        var foundEmail = foundEmails.Single();
        await confirmationEmailService.SendVerificationEmail(account, foundEmail); // returns the verification code (don't need that here)

        res.Data = foundEmail.Value;
        return res;
    }

    public async Task<AuthResponse<string>> VerifyEmail(ClaimsIdentity identity, string code)
    {
        var res = new AuthResponse<string>();
        var account = await GetAccountFromClaims(identity);
        if (account is null)
        {
            res.Errors.Add("failed to find account");
            return res;
        }

        var verifiedEmail = await confirmationEmailService.VerifyEmail(account.Id, code);

        if (verifiedEmail is null)
        {
            res.Errors.Add("Failed to verify email");
            return res;
        }

        res.Data = verifiedEmail.Value;
        return res;
    }

    private static List<Entities.Email> GetAllEmailsFromAccount(Entities.Account account)
    {
        return new List<Entities.Email>(account.OtherEmails)
        {
            account.Email
        };
    }

    private async Task<Entities.Account> GetAccountFromClaims(ClaimsIdentity identity)
    {
        var accountIdClaim = identity.Claims.FirstOrDefault(c => c.Type.Equals("AccountId"));
        var parsed = long.TryParse(accountIdClaim?.Value, out var accountId);

        if (!parsed)
        {
            throw new Exception("Failed to parse accountId from claims");
        }

        var account = await accountRepository.GetAccountById(accountId);

        if (account is null)
        {
            throw new Exception("Failed to find account based on accountId in claims");
        }

        return account;
    }
}