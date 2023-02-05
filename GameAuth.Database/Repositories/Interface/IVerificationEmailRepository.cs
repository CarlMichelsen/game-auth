using GameAuth.Database.Models.Entities;

namespace GameAuth.Database.Repository.Interface;

public interface IVerificationEmailRepository
{
    Task<IEnumerable<VerificationEmail>> GetVerificationEmailsByAccountId(long accountId);
    Task<VerificationEmail?> GetVerificationEmailById(long id);
    Task<VerificationEmail?> GetVerificationEmailByEmailId(long emailId);
    Task<int> DeleteVerificationEmailByAccountId(long accountId); // true if something was deleted
    Task<int> DeleteVerificationEmailByEmailId(long emailId); // true if something was deleted
    Task<string> UpsertNewVerificationEmail(long accountId, long emailId, bool omitDeletion = false);
}