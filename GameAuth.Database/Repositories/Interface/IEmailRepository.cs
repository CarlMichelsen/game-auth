using Entities = GameAuth.Database.Models.Entities;

namespace GameAuth.Database.Repository.Interface;

public interface IEmailRepository
{
    Task<Entities.Email?> GetEmail(long emailId);
    Task<bool> VerifyEmail(long emailId);
}