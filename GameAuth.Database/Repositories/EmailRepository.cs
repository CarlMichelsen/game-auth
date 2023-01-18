using GameAuth.Database.DataContexts;
using GameAuth.Database.Models.Entities;
using GameAuth.Database.Repository.Interface;
using Microsoft.EntityFrameworkCore;

namespace GameAuth.Database.Repository;

public class EmailRepository : IEmailRepository
{
    private readonly DataContext context;

    public EmailRepository(DataContext context)
    {
        this.context = context;
    }

    public async Task<IEnumerable<Email>> GetEmails(string email)
    {
        return await context.Email
            .Where(e => e.Value.Equals(email))
            .ToListAsync();
    }

    public async Task<Email?> GetPrimaryEmail(string email)
    {
        return await context.Email
            .Where(e => e.IsPrimary && e.Value.Equals(email))
            .SingleOrDefaultAsync();
    }
}