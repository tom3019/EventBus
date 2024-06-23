using System.Reflection;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using EasyNetQ;
using EasyNetQ.DI;
using EasyNetQ.Topology;
using EventBus.RabbitMq.Attributes;
using EventBus.RabbitMq.Extensions;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace EventBus.RabbitMq;

public class EasyNetQEventBus : IEventBus
{
    private readonly IBus _bus;
    private readonly RabbitMqOption _rabbitMqOption;

    public EasyNetQEventBus(RabbitMqOption rabbitMqOption)
    {
        _rabbitMqOption = rabbitMqOption;
        _bus = ConfigEventBus();
    }
    
    private IBus ConfigEventBus()
    {
        var connectionConfiguration = this._rabbitMqOption.ToConnectionConfiguration();
            
        return RabbitHutch.CreateBus
        (
            connectionConfiguration,
            services => services.Register<IConventions>
            (
                c => new Conventions(c.Resolve<ITypeNameSerializer>())
            )
        );
            
    }

    /// <summary>
    /// 發佈事件
    /// </summary>
    /// <param name="event">事件</param>
    /// <param name="cancellationToken">cancellationToken</param>
    /// <typeparam name="TEvent">Event Type </typeparam>
    /// <returns></returns>
    public async Task PublishAsync<TEvent>(TEvent @event, CancellationToken cancellationToken = default) where TEvent : class
    {
        if (@event is null)
        {
            throw new ArgumentNullException(nameof(@event));
        }
        
        var eventType = @event.GetType();
        var eventNames = eventType.Name;
        
        var serializerOptions = new JsonSerializerOptions
        {
            Encoder = JavaScriptEncoder
                .UnsafeRelaxedJsonEscaping
        };
        var eventContent = JsonSerializer.Serialize(@event, eventType, serializerOptions);
        
        cancellationToken.ThrowIfCancellationRequested();
        var internalEvent = new InternalEvent
        {
            Id = Guid.NewGuid(),
            EventTypeName = eventNames,
            EventContent = eventContent,
            CreateAt = DateTime.UtcNow
        };
        var internalEventContent = JsonSerializer.Serialize(internalEvent);

        var exchange = GetOrDeclareExchange<TEvent>(_bus.Advanced);
        var eventAttribute = typeof(TEvent).GetCustomAttribute<EventAttribute>();
        var priority = eventAttribute?.Priority ?? 0;
        var routingKey = eventAttribute != null ? eventAttribute.RoutingKey : internalEvent.EventTypeName;

        using var advancedBus = _bus.Advanced;
        
        await advancedBus.PublishAsync
        (
            exchange,
            routingKey,
            true,
            new MessageProperties
            {
                DeliveryMode = (byte)DeliveryMode.Persistent,
                Priority = priority
            },
            Encoding.UTF8.GetBytes(internalEventContent), 
            cancellationToken
        );
    }
    
    /// <summary>
    /// Gets the or declare exchange using the specified advanced bus
    /// </summary>
    /// <typeparam name="TEvent">The event</typeparam>
    /// <param name="advancedBus">The advanced bus</param>
    /// <returns>The exchange</returns>
    private Exchange GetOrDeclareExchange<TEvent>(IAdvancedBus advancedBus)
    {
        var exchangeAttribute = typeof(TEvent).GetCustomAttribute<ExchangeAttribute>();

        var exchangeName = exchangeAttribute != null
            ? exchangeAttribute.ExchangeName
            : _rabbitMqOption.ExchangeName;

        var exchangeType = exchangeAttribute != null ? exchangeAttribute.ExchangeType : ExchangeType.Direct;

        return advancedBus.ExchangeDeclare
        (
            exchangeName,
            exchangeType,
            _rabbitMqOption.Durable,
            _rabbitMqOption.AutoDelete
        );
    }

    public void Subscribe<TEvent, TEventHandler>() where TEvent : class where TEventHandler : IEventHandler<TEvent>
    {
        throw new NotImplementedException();
    }

    public void Unsubscribe<TEvent, TEventHandler>() where TEvent : class where TEventHandler : IEventHandler<TEvent>
    {
        throw new NotImplementedException();
    }
}