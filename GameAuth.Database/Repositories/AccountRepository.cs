using GameAuth.Database.DataContexts;
using GameAuth.Database.Models.Entities;
using GameAuth.Database.Repository.Interface;
using Microsoft.EntityFrameworkCore;

namespace GameAuth.Database.Repository;

public class AccountRepository : IAccountRepository
{
    private readonly DataContext context;

    public AccountRepository(DataContext context)
    {
        this.context = context;
    }

    public async Task<Account?> GetAccountByEmail(string email)
    {
        var foundEmail = await context.Email.Where(e => e.IsPrimary && e.Value.Equals(email)).FirstOrDefaultAsync();
        if (foundEmail is null) return default;

        var account = await context.Account
            .Where(a => a.Id == foundEmail.AccountId)
            .Include(a => a.Emails)
            .FirstOrDefaultAsync();
        if (account is not null) return account;
        return default;
    }

    public async Task<Account> RegisterAccount(Account account)
    {
        var addedAccount = await context.Account.AddAsync(account);
        await context.SaveChangesAsync();
        return addedAccount.Entity;
    }
}