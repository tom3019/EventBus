namespace EventBus.Subscriptions;

public interface ISubscriptionProvider
{
    /// <summary>
    /// 取的事件處理程序
    /// </summary>
    /// <typeparam name="TEvent"></typeparam>
    /// <returns></returns>
    IEnumerable<IEventHandler<TEvent>> GetEventHandlers<TEvent>() where TEvent : class;
}