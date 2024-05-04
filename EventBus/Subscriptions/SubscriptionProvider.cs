namespace EventBus.Subscriptions;

internal class SubscriptionProvider : ISubscriptionProvider
{
    private readonly ISubscriptionCollection _subscriptionCollection;

    internal SubscriptionProvider(ISubscriptionCollection subscriptionCollection)
    {
        _subscriptionCollection = subscriptionCollection;
    }

    public IEnumerable<IEventHandler<TEvent>> GetEventHandlers<TEvent>() where TEvent : class
    {
        return  _subscriptionCollection.Where(s => s.EventType == typeof(TEvent))
            .Select(a => a.HandlerType as IEventHandler<TEvent>);
    }
}