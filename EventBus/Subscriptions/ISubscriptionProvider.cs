namespace EventBus.Subscriptions;

public interface ISubscriptionProvider
{
    IEnumerable<IEventHandler<TEvent>> GetEventHandlers<TEvent>() where TEvent : class;
}