using GameAuth.Api.Models.Dto;
using GameAuth.Api.Models.Dto.Register;

namespace GameAuth.Api.Handlers.Interface;

public interface IRegisterHandler
{
    Task<AuthResponse<TokenResponse>> Register(RegisterRequest request);
}