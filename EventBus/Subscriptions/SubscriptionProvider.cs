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
    /// 取的事件處理程序
    /// </summary>
    /// <typeparam name="TEvent"></typeparam>
    /// <returns></returns>
    public IEnumerable<IEventHandler<TEvent>> GetEventHandlers<TEvent>() where TEvent : class
    {
        var eventHandlers = _serviceProvider.GetServices<IEventHandler<TEvent>>();
        return eventHandlers;
    }
}