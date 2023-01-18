using GameAuth.Database.DataContexts;
using GameAuth.Database.Models.Entities;
using GameAuth.Database.Repository.Interface;
using Microsoft.EntityFrameworkCore;

namespace GameAuth.Database.Repository;

public class BanRepository : IBanRepository
{
    private readonly DataContext context;

    public BanRepository(DataContext context)
    {
        this.context = context;
    }

    public async Task<Ban?> GetBan(long id)
    {
        return await context.Ban.FindAsync(id);
    }

    public async Task<Ban?> IsBanned(long accountId)
    {
        return await context.Ban
            .Where(b => b.AccountId == accountId)
            .FirstOrDefaultAsync();
    }
}