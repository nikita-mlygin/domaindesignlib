using DomainDesignLib.Abstractions;
using Microsoft.Extensions.DependencyInjection;

namespace DomainDesignLib.KafkaBus;

public static class DependencyInjection
{
    public static IServiceCollection AddProducerNames(
        this IServiceCollection services,
        Action<IProducerNameConfigBuilder> builderAction
    )
    {
        services.AddSingleton<IProducerNameResolver, ProducerNameResolver>();
        services.AddSingleton<IBus, KafkaBus>();

        var builder = new ProducerNameConfigurationBuilder();

        builderAction(builder);

        var config = builder.Build();

        services.AddSingleton(config);

        return services;
    }
}
