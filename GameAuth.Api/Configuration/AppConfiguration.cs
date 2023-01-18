namespace GameAuth.Api.Configuration;

public class AppConfiguration : IAppConfiguration, IJwtConfiguration
{
    private ConfigurationManager? configManager;

    public void InitializeConfiguration(ConfigurationManager configManager)
    {
        this.configManager = configManager;
    }

    public string Issuer => ReadConfiguration("Jwt:Issuer");

    public string Audience => ReadConfiguration("Jwt:Audience");

    public string AuthSecret => ReadConfiguration("Jwt:AuthSecret");

    public string RefreshSecret => ReadConfiguration("Jwt:RefreshSecret");

    private string ReadConfiguration(string key)
    {
        if (configManager is null) throw new NullReferenceException("Configuration manager is null");
        return configManager[key] ?? throw new NullReferenceException(key);
    }
}