using GameAuth.Api.Services.Interface;
using GameAuth.Api.Configuration;
using GameAuth.Api.Models.Dto;
using GameAuth.Database.Models.Entities;

using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace GameAuth.Api.Services;

public class JwtService : IJwtService
{
    private readonly IJwtConfiguration config;

    public JwtService(IJwtConfiguration config)
    {
        this.config = config;
    }

    public TokenResponse CreateIdentityToken(Account account)
    {
        var claims = CreateClaims(account);
        return new TokenResponse
        {
            IdentityToken = CreateToken(claims, config.IdentitySecret)
        };
    }

    private static DateTime NewIdentityExpiration()
    {
        return DateTime.UtcNow.AddDays(14);
    }

    private string CreateToken(Claim[] claims, string secret)
    {
        var key = Encoding.ASCII.GetBytes(secret);
        var symmetricKey = new SymmetricSecurityKey(key);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims.Append(new Claim("kid", "Identity"))),
            Expires = NewIdentityExpiration(),
            Issuer = config.Issuer,
            Audience = config.DefaultAudience,
            SigningCredentials = new SigningCredentials(symmetricKey, SecurityAlgorithms.HmacSha512Signature)
        };
        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescriptor);
        var stringToken = tokenHandler.WriteToken(token);
        return stringToken;
    }

    private static Claim[] CreateClaims(Account account)
    {
        if (account.Id == 0) throw new Exception("AccountId 0 can not exist");
        return new[] {
            new Claim("AccountId", account.Id.ToString())
        };
    }
}