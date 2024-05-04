namespace EventBus;

/// <summary>
/// 事件處理程序
/// </summary>
/// <typeparam name="TEvent"></typeparam>
public interface IEventHandler<in TEvent> where TEvent : class
{
    /// <summary>
    /// 處理程序
    /// </summary>
    /// <param name="event"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task HandleAsync(TEvent @event, CancellationToken cancellationToken = default);
}