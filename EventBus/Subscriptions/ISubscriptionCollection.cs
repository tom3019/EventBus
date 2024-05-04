namespace EventBus.Subscriptions;

/// <summary>
/// Subscription Collection Interface
/// </summary>
public interface ISubscriptionCollection : IList<SubscriptionDescriptor>
{
    /// <summary>
    /// Add SubscriptionDescriptor
    /// </summary>
    /// <typeparam name="TEvent"></typeparam>
    /// <typeparam name="TEventHandler"></typeparam>
    void Add<TEvent, TEventHandler>()
        where TEvent : class
        where TEventHandler : IEventHandler<TEvent>;
}