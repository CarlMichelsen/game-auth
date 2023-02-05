using GameAuth.Api.Models.Dto;
using GameAuth.Api.Models.Dto.Login;

namespace GameAuth.Api.Handlers.Interface;

public interface ILoginHandler
{
    Task<AuthResponse<TokenResponse>> Login(LoginRequest request);
}