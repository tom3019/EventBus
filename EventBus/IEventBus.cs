namespace EventBus;

/// <summary>
/// 事件總線
/// </summary>
public interface IEventBus
{
    /// <summary>
    /// 發佈事件
    /// </summary>
    /// <param name="event">事件</param>
    /// <param name="cancellationToken">cancellationToken</param>
    /// <typeparam name="TEvent">Event Type </typeparam>
    /// <returns></returns>
    Task PublishAsync<TEvent>(TEvent @event,CancellationToken cancellationToken = default) where TEvent : class;
    
    /// <summary>
    /// 訂閱事件
    /// </summary>
    /// <typeparam name="TEvent">Event Type</typeparam>
    /// <typeparam name="TEventHandler">Event Handler Type</typeparam>
    void Subscribe<TEvent, TEventHandler>()
        where TEvent : class
        where TEventHandler : IEventHandler<TEvent>;
    
    /// <summary>
    /// 取消訂閱事件
    /// </summary>
    /// <typeparam name="TEvent">Event Type</typeparam>
    /// <typeparam name="TEventHandler">Event Handler Type</typeparam>
    void Unsubscribe<TEvent, TEventHandler>()
        where TEvent : class
        where TEventHandler : IEventHandler<TEvent>;
}