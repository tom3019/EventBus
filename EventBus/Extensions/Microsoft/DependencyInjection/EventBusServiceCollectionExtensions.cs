﻿using EventBus.Background;
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
        var eventBusBuilder = new EventBusBuilder(serviceCollection);
        eventBusBuilder.ServiceCollection.AddScoped<ISubscriptionProvider, SubscriptionProvider>();
        eventBusBuilder.ServiceCollection.AddScoped<IEventHandlerInvoker, EventHandlerInvoker>();
        eventBusBuilder.ServiceCollection.AddHostedService<SubscriptionBackgroundService>();
        setup.Invoke(eventBusBuilder);
        return serviceCollection;
    }
}