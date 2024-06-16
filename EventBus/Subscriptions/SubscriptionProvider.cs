using Microsoft.Extensions.DependencyInjection;

namespace EventBus.Subscriptions;

internal class SubscriptionProvider : ISubscriptionProvider
{
    private readonly IServiceProvider _serviceProvider;

    public SubscriptionProvider(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    /// <summary>
    /// 取得事件處理程序
    /// </summary>
    /// <typeparam name="TEvent"></typeparam>
    /// <returns></returns>
    public IEnumerable<IEventHandler<TEvent>> GetEventHandlers<TEvent>() where TEvent : class
    {
        var eventHandlers = _serviceProvider.GetServices<IEventHandler<TEvent>>();
        return eventHandlers;
    }
    
    /// <summary>
    /// 取得事件處理程序
    /// </summary>
    /// <param name="event"></param>
    /// <returns></returns>
    public IEnumerable<object> GetEventHandlers(object @event)
    {
        var eventHandlerType = typeof(IEventHandler<>).MakeGenericType(@event.GetType());
        var eventHandlers = _serviceProvider.GetServices(eventHandlerType);
        return eventHandlers;
    }
    
}