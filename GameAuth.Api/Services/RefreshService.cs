using GameAuth.Api.Services.Interface;
using System.Security.Claims;
using GameAuth.Api.Models.Dto;

namespace GameAuth.Api.Services;


public class RefreshService : IRefreshService
{
    public Task<AuthResponse<TokenResponse>> Refresh(ClaimsIdentity? claimsIdentity)
    {
        var accountId = claimsIdentity?.Claims.FirstOrDefault(a => a.Type.Equals("AccountId"))?.Value ?? "<No AccountId>";
        var str = $"accountId <{accountId}> attempted to refresh their token";
        Console.WriteLine(str);
        throw new NotImplementedException(str);
    }
}