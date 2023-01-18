using System.Security.Claims;
using GameAuth.Api.Models.Dto;
using GameAuth.Database.Models.Entities;

namespace GameAuth.Api.Services.Interface;

public interface IJwtService
{
    public TokenResponse? CreateJwtSet(Account account);
    public Task<TokenResponse?> RefreshAccess(ClaimsIdentity claimsIdentity);
}