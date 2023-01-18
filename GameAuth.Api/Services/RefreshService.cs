using GameAuth.Api.Services.Interface;
using System.Security.Claims;
using GameAuth.Api.Models.Dto;

namespace GameAuth.Api.Services;


public class RefreshService : IRefreshService
{
    private readonly IJwtService jwtService;
    private readonly IAccessControlService accessControlService;

    public RefreshService(IJwtService jwtService, IAccessControlService accessControlService)
    {
        this.jwtService = jwtService;
        this.accessControlService = accessControlService;
    }

    public async Task<AuthResponse<TokenResponse>> Refresh(ClaimsIdentity? claimsIdentity, string rawRefreshToken)
    {
        var res = new AuthResponse<TokenResponse>();
        if (claimsIdentity is null)
        {
            res.Errors.Add("invalid token");
            return res;
        }

        var access = await accessControlService.AllowAccess(claimsIdentity);
        if (!access)
        {
            res.Errors.Add("banned");
            return res;
        }

        res.Data = await jwtService.RefreshAccess(claimsIdentity, rawRefreshToken);
        return res;
    }
}