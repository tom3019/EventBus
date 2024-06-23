using EventBus.Subscriptions;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace EventBus.Extensions.Microsoft.DependencyInjection;

/// <summary>
/// EventBusBuilderExtensions
/// </summary>
public static class EventBusBuilderExtensions
{
    public static IEventBusBuilder UseSubscriptions(this IEventBusBuilder eventBusBuilder)
    {
        eventBusBuilder.ServiceCollection.TryAddSingleton<ISubscriptionCollection, SubscriptionCollection>();
        return eventBusBuilder;
    }
    
    public static IEventBusBuilder UseSubscriptions(this IEventBusBuilder eventBusBuilder,Action<ISubscriptionBuilder> setup)
    {
        setup.Invoke(new SubscriptionBuilder(eventBusBuilder.ServiceCollection));
        return eventBusBuilder;
    }
}