using GameAuth.Api.Models.Dto;
using GameAuth.Api.Models.Dto.Register;

namespace GameAuth.Api.Services.Interface;

public interface IRegisterService
{
    public Task<AuthResponse<TokenResponse>> Register(RegisterRequest request);
}