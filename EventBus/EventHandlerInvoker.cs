using EventBus.Subscriptions;

namespace EventBus;

public class EventHandlerInvoker : IEventHandlerInvoker
{
    private readonly ISubscriptionProvider _subscriptionProvider;

    public EventHandlerInvoker(ISubscriptionProvider subscriptionProvider)
    {
        _subscriptionProvider = subscriptionProvider;
    }

    /// <summary>
    /// 觸發EventHandlers
    /// </summary>
    /// <param name="event"></param>
    /// <param name="cancellationToken"></param>
    /// <typeparam name="TEvent"></typeparam>
    /// <returns></returns>
    public Task InvokeAsync<TEvent>(TEvent @event, CancellationToken cancellationToken = default) where TEvent : class
    {
        if (@event is null)
        {
            throw new ArgumentNullException(nameof(@event));
        }
        
        var eventHandlers = _subscriptionProvider.GetEventHandlers<TEvent>();
        foreach (var eventHandler in eventHandlers)
        {
            eventHandler.HandleAsync(@event, cancellationToken);
        }
        
        return Task.CompletedTask;
    }

    /// <summary>
    /// 觸發EventHandlers
    /// </summary>
    /// <param name="event"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task InvokeAsync(object @event, CancellationToken cancellationToken = default)
    {
        if (@event is null)
        {
            throw new ArgumentNullException(nameof(@event));
        }
        
        var eventHandlers = _subscriptionProvider.GetEventHandlers(@event);
        foreach (var eventHandler in eventHandlers)
        {
            var method = eventHandler.GetType().GetMethod(nameof(IEventHandler<object>.HandleAsync));
            
            var arguments = new[]
            {
                @event,
                cancellationToken
            };
            method!.Invoke(eventHandler, arguments);
        }
        
        return Task.CompletedTask;
    }
}