using System.Reflection;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using EasyNetQ;
using EasyNetQ.DI;
using EasyNetQ.Topology;
using EventBus.RabbitMq.Attributes;
using EventBus.RabbitMq.Extensions;
using EventBus.Subscriptions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace EventBus.RabbitMq;

public class EasyNetQEventBus : IEventBus
{
    private readonly IBus _bus;
    private readonly RabbitMqOption _rabbitMqOption;
    private readonly ISubscriptionCollection _subscriptionCollection;
    private readonly IServiceScopeFactory _serviceScopeFactory;
    private readonly ILogger<EasyNetQEventBus> _logger;

    public EasyNetQEventBus(IOptions<RabbitMqOption> options ,
        ISubscriptionCollection subscriptionCollection, 
        IServiceScopeFactory serviceScopeFactory, 
        ILogger<EasyNetQEventBus> logger)
    {
        _rabbitMqOption = options.Value;
        _subscriptionCollection = subscriptionCollection;
        _serviceScopeFactory = serviceScopeFactory;
        _logger = logger;
        _bus = ConfigEventBus();
    }

    /// <summary>
    /// 設定EasyNetQ EventBus
    /// </summary>
    /// <returns></returns>
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
    public async Task PublishAsync<TEvent>(TEvent @event, CancellationToken cancellationToken = default)
        where TEvent : class
    {
        if (@event is null)
        {
            throw new ArgumentNullException(nameof(@event));
        }

        var eventType = @event.GetType();
        var eventNames = eventType.Name;
        _logger.LogTrace("publishing event ({Name})...", @event.GetType().Name);
        
        var serializerOptions = new JsonSerializerOptions
        {
            Encoder = JavaScriptEncoder
                .UnsafeRelaxedJsonEscaping
        };
        var eventContent = JsonSerializer.Serialize(@event, eventType, serializerOptions);

        _logger.LogInformation("publish event message is ({EventContent})...", eventContent);
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

        using var advancedBus = _bus.Advanced;

        await advancedBus.PublishAsync
        (
            exchange,
            internalEvent.EventTypeName,
            true,
            new MessageProperties
            {
                DeliveryMode = (byte)DeliveryMode.Persistent,
                Priority = priority
            },
            Encoding.UTF8.GetBytes(internalEventContent),
            cancellationToken
        );
        
        _logger.LogTrace("Published event");
    }


    /// <summary>
    /// 訂閱事件
    /// </summary>
    /// <typeparam name="TEvent">Event Type</typeparam>
    /// <typeparam name="TEventHandler">Event Handler Type</typeparam>
    public void Subscribe<TEvent, TEventHandler>() where TEvent : class where TEventHandler : IEventHandler<TEvent>
    {
        var containsKey =
            _subscriptionCollection.Contains(new SubscriptionDescriptor(typeof(TEvent), typeof(TEventHandler)));
        if (containsKey)
        {
            return;
        }

        var eventName = typeof(TEvent).Name;
        var eventHandlerName = typeof(TEventHandler).Name;
        _logger.LogInformation("Subscribing to event {EventName} with {EventHandlerName}...", eventName, eventHandlerName);
       
        using var advancedBus = _bus.Advanced;
        
        var exchange = GetOrDeclareExchange<TEvent>(advancedBus);
        var queue = GetOrDeclareQueue<TEvent>(advancedBus);
        
        advancedBus.Bind(exchange, queue, eventName);
        advancedBus.Consume
        (
            queue,
            Consumer_Received,
            config =>
            {
                if (this._rabbitMqOption.ConsumePrefetchCount.Equals(default).Equals(false))
                {
                    config.WithPrefetchCount(this._rabbitMqOption.ConsumePrefetchCount);
                }
            }
        );

        _subscriptionCollection.Add<TEvent, TEventHandler>();
        _logger.LogInformation("Subscribed to event {EventName} with {EventHandlerName}...", eventName, eventHandlerName);
    }

    /// <summary>
    /// 取消訂閱事件
    /// </summary>
    /// <typeparam name="TEvent">Event Type</typeparam>
    /// <typeparam name="TEventHandler">Event Handler Type</typeparam>
    public void Unsubscribe<TEvent, TEventHandler>() where TEvent : class where TEventHandler : IEventHandler<TEvent>
    {
        using var advancedBus = _bus.Advanced;

        var eventName = typeof(TEvent).Name;
        var eventHandlerName = typeof(TEventHandler).Name;
        _logger.LogInformation("Unsubscribing {EventHandler} from event {EventName}...",eventHandlerName ,eventName);
        advancedBus.QueueUnbindAsync
            (
                _rabbitMqOption.QueueName,
                _rabbitMqOption.ExchangeName,
                eventName,
                new Dictionary<string, object>()
            )
            .ConfigureAwait(false)
            .GetAwaiter()
            .GetResult();

        _subscriptionCollection.Remove(new SubscriptionDescriptor(typeof(TEvent), typeof(TEventHandler)));
        _logger.LogInformation("Unsubscribed {EventHandler} from event {EventName}", eventHandlerName ,eventName);
    }

    /// <summary>
    /// 訂閱事件接收
    /// </summary>
    /// <param name="readOnlyMemory"></param>
    /// <param name="messageProperties"></param>
    /// <param name="messageReceivedInfo"></param>
    private async Task Consumer_Received(ReadOnlyMemory<byte> readOnlyMemory, MessageProperties messageProperties,
        MessageReceivedInfo messageReceivedInfo)
    {

        using var serviceScope = _serviceScopeFactory.CreateScope();

        var serviceProvider = serviceScope.ServiceProvider;
        var eventHandlerInvoker = serviceProvider.GetRequiredService<IEventHandlerInvoker>();
        
        var internalEventContent = Encoding.UTF8.GetString(readOnlyMemory.Span);
        var internalEvent = JsonSerializer.Deserialize<InternalEvent>(internalEventContent);

        var eventType = _subscriptionCollection
            .FirstOrDefault(x => x.EventType.Name == internalEvent.EventTypeName)!.EventType;

        var serializerOptions = new JsonSerializerOptions
        {
            Encoder = JavaScriptEncoder
                .UnsafeRelaxedJsonEscaping
        };
        var @event = JsonSerializer.Deserialize(internalEvent.EventContent, eventType, serializerOptions);
        await eventHandlerInvoker.InvokeAsync(@event);
    }

    /// <summary>
    /// 取得或宣告Queue
    /// </summary>
    /// <typeparam name="TEvent">The type of the event.</typeparam>
    /// <returns>The queue for the event.</returns>
    private Queue GetOrDeclareQueue<TEvent>(IAdvancedBus advancedBus) where TEvent : class
    {
        var queueAttribute = typeof(TEvent).GetCustomAttribute<Attributes.QueueAttribute>();

        var queueName = queueAttribute != null ? queueAttribute.QueueName : _rabbitMqOption.QueueName;

        var durable = queueAttribute != null ? queueAttribute.Durable : _rabbitMqOption.Durable;

        var exclusive = queueAttribute != null ? queueAttribute.Exclusive : false;

        var autoDelete = queueAttribute != null ? queueAttribute.AutoDelete : _rabbitMqOption.AutoDelete;

        return advancedBus.QueueDeclare
        (
            queueName,
            configure =>
            {
                configure.AsDurable(durable)
                    .AsExclusive(exclusive)
                    .AsAutoDelete(autoDelete);

                if (this._rabbitMqOption.MaxPriority.Equals(0).Equals(false))
                {
                    configure.WithArgument("x-max-priority", this._rabbitMqOption.MaxPriority);
                }
            }
        );
    }

    /// <summary>
    /// 取得Exchange
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
}