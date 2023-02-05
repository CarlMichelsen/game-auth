using GameAuth.Email.Configuration;

namespace GameAuth.Api.Configuration;

public class AppConfiguration : IAppConfiguration,
    IJwtConfiguration,
    IEmailConfiguration
{
    private ConfigurationManager? configManager;

    public void InitializeConfiguration(ConfigurationManager configManager)
    {
        this.configManager = configManager;
    }

    public string Issuer => ReadConfiguration("Jwt:Issuer");

    public string DefaultAudience => ReadConfiguration("Jwt:DefaultAudience");

    public string IdentitySecret => ReadConfiguration("Jwt:IdentitySecret");

    public string MailApiKey => ReadConfiguration("MailGun:ApiKey");

    private string ReadConfiguration(string key)
    {
        if (configManager is null) throw new NullReferenceException("Configuration manager is null");
        return configManager[key] ?? throw new NullReferenceException(key);
    }
}