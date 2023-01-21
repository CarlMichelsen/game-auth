using System.Security.Claims;
using GameAuth.Api.Models.Dto;

namespace GameAuth.Api.Services.Interface;

public interface IRefreshService
{
    Task<AuthResponse<TokenResponse>> Refresh(ClaimsIdentity? claimsIdentity, string rawRefreshToken);
}