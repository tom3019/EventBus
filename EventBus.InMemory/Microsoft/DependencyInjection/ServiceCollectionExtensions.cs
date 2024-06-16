using EventBus.Extensions.Microsoft.DependencyInjection;
using EventBus.InMemory.Background;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace EventBus.InMemory.Microsoft.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IEventBusBuilder UseInMemory(this IEventBusBuilder eventBusBuilder, int capacity = 100)
    {
        eventBusBuilder.ServiceCollection.TryAddSingleton<IBackgroundQueue<InternalEvent>>
        (
            _ => new BackgroundQueue<InternalEvent>(capacity)
        );
        eventBusBuilder.ServiceCollection.AddHostedService<SubscriberBackgroundService>();
        eventBusBuilder.ServiceCollection.TryAddSingleton<IEventBus, InMemoryEventBus>();
        return eventBusBuilder;
    }
}