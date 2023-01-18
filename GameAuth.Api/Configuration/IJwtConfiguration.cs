namespace GameAuth.Api.Configuration;

public interface IJwtConfiguration
{
    public string Issuer { get; }
    public string Audience { get; }
    public string AuthSecret { get; }
    public string RefreshSecret { get; }
}