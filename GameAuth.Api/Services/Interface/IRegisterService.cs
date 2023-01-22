using System.Security.Claims;
using GameAuth.Api.Models.Dto;
using GameAuth.Api.Models.Dto.Register;

namespace GameAuth.Api.Services.Interface;

public interface IRegisterService
{
    Task<AuthResponse<TokenResponse>> Register(RegisterRequest request);
    Task<AuthResponse<bool>> ResendVerificationEmail(ClaimsIdentity identity);
}