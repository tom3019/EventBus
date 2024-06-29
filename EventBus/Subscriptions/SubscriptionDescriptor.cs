namespace EventBus.Subscriptions;

/// <summary>
/// Subscription Descriptor
/// </summary>
public record SubscriptionDescriptor
{
    /// <summary>
    /// Initializes a new instance of the <see cref="SubscriptionDescriptor" /> class
    /// </summary>
    /// <param name="eventType">event type</param>
    /// <param name="handlerType">event handler type</param>
    public SubscriptionDescriptor(Type eventType, Type handlerType)
    {
        EventType = eventType;
        HandlerType = handlerType;
    }

    /// <summary>
    /// Event Type
    /// </summary>
    public Type EventType { get; }

    /// <summary>
    /// Event Handler Type
    /// </summary>
    public Type HandlerType { get; }
}