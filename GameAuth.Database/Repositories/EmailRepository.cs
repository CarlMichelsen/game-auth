using GameAuth.Database.DataContexts;
using GameAuth.Database.Repository.Interface;
using Entities = GameAuth.Database.Models.Entities;

namespace GameAuth.Database.Repository;

public class EmailRepository : IEmailRepository
{
    private readonly DataContext context;

    public EmailRepository(DataContext context)
    {
        this.context = context;
    }

    public async Task<Entities.Email?> GetEmail(long emailId)
    {
        return await context.Email.FindAsync(emailId);
    }

    public async Task<bool> VerifyEmail(long emailId)
    {
        try
        {
            var email = context.Email.Find(emailId);
            if (email is not null)
            {
                email.Verified = true;
            }
            else
            {
                return false;
            }
            await context.SaveChangesAsync();
            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }
}