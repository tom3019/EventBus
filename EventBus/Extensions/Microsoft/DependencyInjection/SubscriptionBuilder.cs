using Microsoft.Extensions.DependencyInjection;

namespace EventBus.Extensions.Microsoft.DependencyInjection;

public class SubscriptionBuilder : ISubscriptionBuilder
{
    /// <summary>
    /// Service collection
    /// </summary>
    public IServiceCollection ServiceCollection { get; set; }



    /// <summary>
    /// EventBusBuilder
    /// </summary>
    /// <param name="serviceCollection"></param>
    public SubscriptionBuilder(IServiceCollection serviceCollection)
    {
        ServiceCollection = serviceCollection;
    }
}