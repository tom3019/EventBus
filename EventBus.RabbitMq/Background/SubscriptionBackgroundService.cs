using EventBus.Subscriptions;
using Microsoft.Extensions.Hosting;

namespace EventBus.RabbitMq.Background;

internal class SubscriptionBackgroundService : BackgroundService
{
    /// <summary>
    /// 訂閱集合
    /// </summary>
    private readonly ISubscriptionCollection _subscriptionCollection;

    /// <summary>
    /// EventBus
    /// </summary>
    private readonly IEventBus _eventBus;
    

    /// <summary>
    /// SubscriptionBackgroundService
    /// </summary>
    /// <param name="subscriptionCollection"></param>
    /// <param name="eventBus"></param>
    public SubscriptionBackgroundService(ISubscriptionCollection subscriptionCollection, 
        IEventBus eventBus)
    {
        _subscriptionCollection = subscriptionCollection;
        _eventBus = eventBus;
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