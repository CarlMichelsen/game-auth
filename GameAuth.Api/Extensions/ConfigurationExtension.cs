using GameAuth.Api.Configuration;

namespace Microsoft.Extensions.DependencyInjection;

public static class ConfigurationExtension
{
    public static IServiceCollection AddConfigurationSingleton<TService, TImplementation>(
        this IServiceCollection serviceCollection,
        ConfigurationManager configurationManager)
        where TService : class
        where TImplementation : TService, IAppConfiguration, new()
    {
        var instance = new TImplementation();
        instance.InitializeConfiguration(configurationManager);
        serviceCollection.AddSingleton<TService>(instance);
        return serviceCollection;
    }
}