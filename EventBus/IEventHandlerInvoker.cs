namespace EventBus;

/// <summary>
///   Event Handler Invoker Interface
/// </summary>
public interface IEventHandlerInvoker
{
    /// <summary>
    ///    Invokes the specified event handlers that are registered to handle the given event asynchronously.
    /// </summary>
    /// <param name="event"></param>
    /// <param name="cancellationToken"></param>
    /// <typeparam name="TEvent"></typeparam>
    /// <returns></returns>
    Task InvokeAsync<TEvent>(TEvent @event, CancellationToken cancellationToken = default);
}