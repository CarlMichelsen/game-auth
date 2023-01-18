using GameAuth.Database.Models.Entities;

namespace GameAuth.Database.Repository.Interface;

public interface IAccountRepository
{
    public Task<Account> RegisterAccount(Account account);
    public Task<Account?> GetAccountByEmail(string email);
}