namespace EventBus;

/// <summary>
///   Event Handler Invoker Interface
/// </summary>
public interface IEventHandlerInvoker
{
    /// <summary>
    /// 觸發EventHandlers
    /// </summary>
    /// <param name="event"></param>
    /// <param name="cancellationToken"></param>
    /// <typeparam name="TEvent"></typeparam>
    /// <returns></returns>
    Task InvokeAsync<TEvent>(TEvent @event, CancellationToken cancellationToken = default) where TEvent : class;

    /// <summary>
    /// 觸發EventHandlers
    /// </summary>
    /// <param name="event"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task InvokeAsync(object @event, CancellationToken cancellationToken = default);
}