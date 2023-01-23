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

    public async Task<bool> EmailExisits(string email)
    {
        return await context.Email
            .Where(e => e.IsPrimary && e.Value.Equals(email))
            .AnyAsync();
    }

    public async Task<Account?> GetAccountByEmail(string email)
    {
        var foundEmail = context.Email.Where(e => e.Value.Equals(email)).Single();
        return await context.Account.FindAsync(foundEmail.AccountId);
    }

    public async Task<Account?> GetAccountById(long id)
    {
        return await context.Account.Include(a => a.Emails).SingleAsync(a => a.Id == id);
    }

    public async Task<Account> RegisterAccount(Account account)
    {
        var addedAccount = await context.Account.AddAsync(account);
        await context.SaveChangesAsync();
        return addedAccount.Entity;
    }

    public async Task<bool> VerifyEmail(long id)
    {
        try
        {
            var account = context.Account.Find(id);
            if (account is null) return false;
            account.EmailVerified = true;
            context.Account.Update(account);
            await context.SaveChangesAsync();
            return true;
        }
        catch (System.Exception)
        {
            return false;
        }
    }
}