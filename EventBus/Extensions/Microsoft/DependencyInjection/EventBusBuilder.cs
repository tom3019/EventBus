using EventBus.Subscriptions;
using Microsoft.Extensions.DependencyInjection;

namespace EventBus.Extensions.Microsoft.DependencyInjection;

/// <summary>
/// event bus builder
/// </summary>
public class EventBusBuilder:IEventBusBuilder
{
    /// <summary>
    /// Service collection
    /// </summary>
    public IServiceCollection ServiceCollection { get; set; }



    /// <summary>
    /// EventBusBuilder
    /// </summary>
    /// <param name="serviceCollection"></param>
    public EventBusBuilder(IServiceCollection serviceCollection)
    {
        ServiceCollection = serviceCollection;
    }
}