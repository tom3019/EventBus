using EventBus.Subscriptions;
using Microsoft.Extensions.Hosting;

namespace EventBus.Background;

internal class SubscriptionBackgroundService : BackgroundService
{
    
    /// <summary>
    ///     The event bus
    /// </summary>
    private readonly IEventBus _eventBus;

    /// <summary>
    /// 訂閱集合
    /// </summary>
    private readonly ISubscriptionCollection _subscriptionCollection;

    /// <summary>
    /// SubscriptionBackgroundService
    /// </summary>
    /// <param name="eventBus"></param>
    /// <param name="subscriptionCollection"></param>
    public SubscriptionBackgroundService(IEventBus eventBus,
        ISubscriptionCollection subscriptionCollection)
    {
        _eventBus = eventBus;
        _subscriptionCollection = subscriptionCollection;
    }

    /// <summary>
    /// ExecuteAsync
    /// </summary>
    /// <param name="stoppingToken"></param>
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        foreach (var subscriptionDescriptor in _subscriptionCollection.ToList())
        {
            typeof(IEventBus).GetMethod(nameof(IEventBus.Subscribe))!
                .MakeGenericMethod(subscriptionDescriptor.EventType, subscriptionDescriptor.HandlerType)
                .Invoke(_eventBus, parameters: null);
        }

        await Task.CompletedTask;
    }
}