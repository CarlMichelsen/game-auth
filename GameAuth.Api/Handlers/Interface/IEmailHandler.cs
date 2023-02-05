using System.Security.Claims;
using GameAuth.Api.Models.Dto;

namespace GameAuth.Api.Handlers.Interface;

public interface IEmailHandler
{
    Task<AuthResponse<string>> ResendVerificationEmail(ClaimsIdentity identity, string email);
    Task<AuthResponse<string>> VerifyEmail(ClaimsIdentity identity, string code);
}