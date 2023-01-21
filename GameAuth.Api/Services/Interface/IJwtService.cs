using System.Security.Claims;
using GameAuth.Api.Models.Dto;
using GameAuth.Database.Models.Entities;

namespace GameAuth.Api.Services.Interface;

public interface IJwtService
{
    TokenResponse? CreateJwtSet(Account account);
    Task<TokenResponse?> RefreshAccess(ClaimsIdentity claimsIdentity, string rawRefreshToken);
}