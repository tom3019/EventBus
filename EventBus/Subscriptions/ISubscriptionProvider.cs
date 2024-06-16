namespace EventBus.Subscriptions;

public interface ISubscriptionProvider
{
    /// <summary>
    /// 取得事件處理程序
    /// </summary>
    /// <typeparam name="TEvent"></typeparam>
    /// <returns></returns>
    IEnumerable<IEventHandler<TEvent>> GetEventHandlers<TEvent>() where TEvent : class;

    /// <summary>
    /// 取得事件處理程序
    /// </summary>
    /// <param name="event"></param>
    /// <returns></returns>
    IEnumerable<object> GetEventHandlers(object @event);
}