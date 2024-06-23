using EventBus.Subscriptions;
using Microsoft.Extensions.Hosting;

namespace EventBus.RabbitMq.BackgroundServices;

internal class SubscriptionBackgroundService : BackgroundService
{
    
    /// <summary>
    ///     The event bus
    /// </summary>
    private readonly IEventBus _eventBus;

    private readonly ISubscriptionCollection _subscriptionCollection;

    public SubscriptionBackgroundService(IEventBus eventBus,
        ISubscriptionCollection subscriptionCollection)
    {
        _eventBus = eventBus;
        _subscriptionCollection = subscriptionCollection;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        foreach (var subscriptionDescriptor in _subscriptionCollection.ToList())
        {
            typeof(IEventBus).GetMethod(nameof(IEventBus.Subscribe))!.MakeGenericMethod(subscriptionDescriptor.EventType, subscriptionDescriptor.HandlerType)
                .Invoke(_eventBus, parameters: null);
        }

        await Task.CompletedTask;
    }
}