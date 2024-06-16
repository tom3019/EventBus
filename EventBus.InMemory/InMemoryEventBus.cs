using System.Text.Encodings.Web;
using System.Text.Json;
using System.Threading.Channels;
using EventBus.InMemory.Background;
using EventBus.Subscriptions;

namespace EventBus.InMemory;

internal class InMemoryEventBus : IEventBus
{
    private readonly IBackgroundQueue<InternalEvent> _backgroundQueue;
    private readonly ISubscriptionCollection _subscriptionCollection;

    public InMemoryEventBus(IBackgroundQueue<InternalEvent> backgroundQueue, 
        ISubscriptionCollection subscriptionCollection)
    {
        _backgroundQueue = backgroundQueue;
        _subscriptionCollection = subscriptionCollection;
    }

    public async Task PublishAsync<TEvent>(TEvent @event, CancellationToken cancellationToken = default) where TEvent : class
    {
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

        await _backgroundQueue.EnqueueAsync(internalEvent);
    }

    public void Subscribe<TEvent, TEventHandler>() where TEvent : class where TEventHandler : IEventHandler<TEvent>
    {
        _subscriptionCollection.Add(new SubscriptionDescriptor(typeof(TEvent), typeof(TEventHandler)));
    }

    public void Unsubscribe<TEvent, TEventHandler>() where TEvent : class where TEventHandler : IEventHandler<TEvent>
    {
        _subscriptionCollection.Remove(new SubscriptionDescriptor(typeof(TEvent), typeof(TEventHandler)));
    }
}