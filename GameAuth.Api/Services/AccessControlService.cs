using System.Security.Claims;
using GameAuth.Api.Services.Interface;
using GameAuth.Database.Repository.Interface;

namespace GameAuth.Api.Services;

public class AccessControlService : IAccessControlService
{
    private readonly ILogger<AccessControlService> logger;
    private readonly IBanRepository banRepository;

    public AccessControlService(ILogger<AccessControlService> logger, IBanRepository banRepository)
    {
        this.logger = logger;
        this.banRepository = banRepository;
    }

    /// <summary>
    /// Guard in front of refresh token refresh
    /// </summary>
    public async Task<bool> AllowAccess(ClaimsIdentity claimsIdentity)
    {
        try
        {
            var stringAccountId = claimsIdentity?.Claims.FirstOrDefault(a => a.Type.Equals("AccountId"))?.Value
                ?? throw new NullReferenceException("No AccountId in refreshtoken");

            var validLong = long.TryParse(stringAccountId, out var accountId);
            if (!validLong) return false;

            var potentialBan = await banRepository.IsBanned(accountId);
            if (potentialBan is null) return true;

            return false;
        }
        catch (Exception e)
        {
            logger.LogCritical(
                "Refresh access check triggered an exception \"{}\" [{}]",
                e.Message,
                e.Source
            );
            return false;
        }
    }
}