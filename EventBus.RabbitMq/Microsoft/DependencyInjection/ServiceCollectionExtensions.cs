using EventBus.Extensions.Microsoft.DependencyInjection;
using EventBus.RabbitMq.Background;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace EventBus.RabbitMq.Microsoft.DependencyInjection;

public static class ServiceCollectionExtensions
{
    /// <summary>
    /// 使用 RabbitMq
    /// </summary>
    /// <param name="eventBusBuilder"></param>
    /// <returns></returns>
    public static IEventBusBuilder UseRabbitMq(this IEventBusBuilder eventBusBuilder)
    {
        eventBusBuilder.ServiceCollection.AddOptions<RabbitMqOption>()
            .BindConfiguration(nameof(RabbitMqOption));
        eventBusBuilder.AddRabbitMqEventBus();

        return eventBusBuilder;
    }
    
    /// <summary>
    /// 使用 RabbitMq
    /// </summary>
    /// <param name="eventBusBuilder"></param>
    /// <param name="setUp"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
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
        eventBusBuilder.ServiceCollection.AddHostedService<SubscriptionBackgroundService>();
        return eventBusBuilder;
    }
}