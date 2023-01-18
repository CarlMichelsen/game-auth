using System.Security.Claims;

namespace GameAuth.Api.Services.Interface;

public interface IAccessControlService
{
    public Task<bool> AllowAccess(ClaimsIdentity claimsIdentity);
}