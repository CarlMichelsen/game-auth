using System.Security.Claims;
using GameAuth.Api.Models.Dto;

namespace GameAuth.Api.Services.Interface;

public interface IRefreshService
{
    public Task<AuthResponse<TokenResponse>> Refresh(ClaimsIdentity? claimsIdentity);
}