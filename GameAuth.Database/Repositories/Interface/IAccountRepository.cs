using GameAuth.Database.Models.Entities;

namespace GameAuth.Database.Repository.Interface;

public interface IAccountRepository
{
    public Task<Account> RegisterAccount(Account account);
    public Task<Account?> GetAccountByEmail(string email);
    public Task<Account?> GetAccountById(long id);
    public Task<bool> VerifyEmail(long id);
    public Task<bool> EmailExisits(string email);
}