using EventBus.Background;
using EventBus.Extensions.Microsoft.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace EventBus.RabbitMq.Microsoft.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IEventBusBuilder UseRabbitMq(this IEventBusBuilder eventBusBuilder)
    {
        eventBusBuilder.ServiceCollection.AddOptions<RabbitMqOption>()
            .BindConfiguration(nameof(RabbitMqOption));
        eventBusBuilder.AddRabbitMqEventBus();

        return eventBusBuilder;
    }
    
    public static IEventBusBuilder UseRabbitMq(this IEventBusBuilder eventBusBuilder, 
        Action<RabbitMqOption> setUp)
    {
        if (setUp is null)
        {
            throw new ArgumentNullException(nameof(setUp));
        }
        eventBusBuilder.ServiceCollection.AddOptions<RabbitMqOption>()
            .Configure(setUp);

        return eventBusBuilder.AddRabbitMqEventBus();
    }
    
    
    /// <summary>
    ///     Adds the rabbit mq event bus using the specified event bus builder
    /// </summary>
    /// <param name="eventBusBuilder">The event bus builder</param>
    /// <returns>The event bus builder</returns>
    private static IEventBusBuilder AddRabbitMqEventBus(this IEventBusBuilder eventBusBuilder)
    {
        eventBusBuilder.ServiceCollection.TryAddSingleton<IEventBus,EasyNetQEventBus>();
        return eventBusBuilder;
    }
}