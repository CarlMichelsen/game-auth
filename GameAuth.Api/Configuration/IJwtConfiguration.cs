namespace GameAuth.Api.Configuration;

public interface IJwtConfiguration
{
    public string Issuer { get; }
    public string DefaultAudience { get; }
    public string IdentitySecret { get; }
}