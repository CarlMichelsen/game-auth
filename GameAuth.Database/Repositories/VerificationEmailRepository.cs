using System.Text;
using GameAuth.Database.DataContexts;
using GameAuth.Database.Models.Entities;
using GameAuth.Database.Repository.Interface;
using Microsoft.EntityFrameworkCore;

namespace GameAuth.Database.Repository;

public class VerificationEmailRepository : IVerificationEmailRepository
{
    private readonly DataContext context;

    public VerificationEmailRepository(DataContext context)
    {
        this.context = context;
    }

    public Task<int> DeleteVerificationEmailByAccountId(long accountId)
    {
        return context.VerificationEmail
            .Where(e => e.AccountId == accountId)
            .ExecuteDeleteAsync();
    }

    public Task<int> DeleteVerificationEmailByEmailId(long emailId)
    {
        return context.VerificationEmail
            .Where(e => e.EmailId == emailId)
            .ExecuteDeleteAsync();
    }

    public async Task<IEnumerable<VerificationEmail>> GetVerificationEmailsByAccountId(long accountId)
    {
        var list = await context.VerificationEmail
            .Where(e => e.AccountId == accountId)
            .ToListAsync();
        return list;
    }

    public Task<VerificationEmail?> GetVerificationEmailByEmailId(long emailId)
    {
        return context.VerificationEmail
            .Where(e => e.EmailId == emailId)
            .FirstOrDefaultAsync();
    }

    public Task<VerificationEmail?> GetVerificationEmailById(long id)
    {
        return context.VerificationEmail
            .Where(e => e.Id == id)
            .FirstOrDefaultAsync();
    }

    public async Task<string> UpsertNewVerificationEmail(long accountId, long emailId, bool omitDeletion = false)
    {
        var code = GenerateVerificationCode();
        var verEmail = new VerificationEmail
        {
            AccountId = accountId,
            EmailId = emailId,
            Code = code,
            Sent = DateTime.UtcNow
        };

        try
        {
            var email = context.VerificationEmail.Where(e => e.AccountId == accountId).ExecuteDelete();
            var res = context.VerificationEmail.Add(verEmail);
            await context.SaveChangesAsync();
            return res.Entity.Code;
        }
        catch (Exception)
        {
            throw new Exception("Failed to register resent email");
        }
    }

    private static string GenerateVerificationCode()
    {
        var length = 4;
        Random rnd = new();
        StringBuilder sb = new();
        for (int i = 0; i < length; i++)
        {
            var num = rnd.Next(0, 10);
            sb.Append(num);
        }
        return sb.ToString();
    }
}