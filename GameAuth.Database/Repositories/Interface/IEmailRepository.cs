using GameAuth.Database.Models.Entities;

namespace GameAuth.Database.Repository.Interface;

public interface IEmailRepository
{
    public Task<Email?> GetPrimaryEmail(string email);
    public Task<IEnumerable<Email>> GetEmails(string email);
}