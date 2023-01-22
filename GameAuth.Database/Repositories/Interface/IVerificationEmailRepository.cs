using GameAuth.Database.Models.Entities;

namespace GameAuth.Database.Repository.Interface;

public interface IVerificationEmailRepository
{
    Task<VerificationEmail?> GetVerificationEmailById(long id);
    Task<VerificationEmail?> GetVerificationEmailByAccountId(long accountId);
    Task<int> DeleteVerificationEmailByAccountId(long accountId); // true if something was deleted
    Task<string> UpsertNewVerificationEmail(long accountId, bool omitDeletion = false);
}