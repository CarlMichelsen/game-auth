using GameAuth.Api.Models.Dto;
using GameAuth.Database.Models.Entities;

namespace GameAuth.Api.Services.Interface;

public interface IJwtService
{
    TokenResponse? CreateIdentityToken(Account account);
}