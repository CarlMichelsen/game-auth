using GameAuth.Database.Models.Entities;

namespace GameAuth.Database.Repository.Interface;

public interface IBanRepository
{
    public Task<Ban?> IsBanned(long accountId);
    public Task<Ban?> GetBan(long id);
}