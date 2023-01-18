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
    private readonly IAccessControlService accessControlService;

    public JwtService(IJwtConfiguration config, IAccessControlService accessControlService)
    {
        this.config = config;
        this.accessControlService = accessControlService;
    }

    public TokenResponse CreateJwtSet(Account account)
    {
        var claims = CreateClaims(account);
        return new TokenResponse
        {
            AuthToken = CreateToken(claims, config.AuthSecret, "auth", NewAuthExpireTime()),
            RefreshToken = CreateToken(claims, config.RefreshSecret, "refresh")
        };
    }

    public async Task<TokenResponse?> RefreshAccess(ClaimsIdentity claimsIdentity, string rawRefreshToken)
    {
        var allowed = await accessControlService.AllowAccess(claimsIdentity);
        if (!allowed) return default;

        var claims = claimsIdentity.Claims.ToArray();
        return new TokenResponse
        {
            AuthToken = CreateToken(claims, config.AuthSecret, "auth", NewAuthExpireTime()),
            RefreshToken = rawRefreshToken
        };
    }

    private static DateTime NewAuthExpireTime()
    {
        return DateTime.UtcNow.AddMinutes(5);
    }

    private string CreateToken(Claim[] claims, string secret, string kid, DateTime? expires = null)
    {
        var key = Encoding.ASCII.GetBytes(secret);
        var symmetricKey = new SymmetricSecurityKey(key);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims.Append(new Claim("kid", kid))),
            Expires = expires,
            Issuer = config.Issuer,
            Audience = config.Audience,
            SigningCredentials = new SigningCredentials(symmetricKey, SecurityAlgorithms.HmacSha512Signature)
        };
        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescriptor);
        var stringToken = tokenHandler.WriteToken(token);
        return stringToken;
    }

    private static Claim[] CreateClaims(Account account)
    {
        //var primaryEmail = account.Emails.First(e => e.Primary); // There should ALWAYS be a primary email
        if (account.Id == 0) throw new Exception("AccountId 0 can not exist");
        return new[] {
            new Claim("AccountId", account.Id.ToString())
        };
    }
}