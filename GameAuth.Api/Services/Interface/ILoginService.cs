using GameAuth.Api.Models.Dto;
using GameAuth.Api.Models.Dto.Login;

namespace GameAuth.Api.Services.Interface;

public interface ILoginService
{
    Task<AuthResponse<TokenResponse>> Login(LoginRequest request);
}