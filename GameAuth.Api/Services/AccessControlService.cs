using GameAuth.Api.Services.Interface;
using GameAuth.Database.Repository.Interface;

namespace GameAuth.Api.Services;

public class AccessControlService : IAccessControlService
{
    private readonly IBanRepository banRepository;
    public AccessControlService(IBanRepository banRepository)
    {
        this.banRepository = banRepository;
    }

    /// <summary>
    /// Guard in front of refresh token refresh
    /// </summary>
    public async Task<bool> AllowAccess(string stringAccountId)
    {
        var validLong = long.TryParse(stringAccountId, out var accountId);
        if (!validLong) return false;

        var potentialBan = await banRepository.IsBanned(accountId);
        if (potentialBan is null) return true;

        return false;
    }
}