using EventBus.Subscriptions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace EventBus.Extensions.Microsoft.DependencyInjection;

/// <summary>
///    Provides extension methods for registering the event bus in the service collection.
/// </summary>
public static class EventBusServiceCollectionExtensions
{
    /// <summary>
    ///    Adds the event bus to the service collection using the specified services and configuration.
    /// </summary>
    /// <param name="serviceCollection"></param>
    /// <param name="setup"></param>
    /// <returns></returns>
    public static IServiceCollection AddEventBus(this IServiceCollection serviceCollection, Action<IEventBusBuilder> setup)
    {
        serviceCollection.AddScoped<ISubscriptionCollection, SubscriptionCollection>();
        serviceCollection.AddScoped<ISubscriptionProvider, SubscriptionProvider>();
        
        var eventBusBuilder = new EventBusBuilder(serviceCollection);
        setup.Invoke(eventBusBuilder);
        return serviceCollection;
    }
}