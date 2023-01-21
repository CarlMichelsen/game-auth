using System.Security.Claims;

namespace GameAuth.Api.Services.Interface;

public interface IAccessControlService
{
    Task<bool> AllowAccess(ClaimsIdentity claimsIdentity);
}