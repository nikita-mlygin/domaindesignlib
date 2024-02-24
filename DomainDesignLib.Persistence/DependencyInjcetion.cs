namespace DomainDesignLib.Persistence;

using DomainDesignLib.Abstractions.Event;
using DomainDesignLib.Persistence.EntityTracking;
using Microsoft.Extensions.DependencyInjection;

public static class DependencyInjection
{
    public static IServiceCollection AddEventDispatcher(this IServiceCollection services)
    {
        services.AddScoped<IEventDispatcher, EventDispatcher.EventDispatcher>();

        return services;
    }

    public static IServiceCollection AddEntityTracking(this IServiceCollection services)
    {
        services.AddScoped<IEntityTracking, EntityTracking.EntityTracking>();

        return services;
    }
}
