﻿using System.Text.Encodings.Web;
using System.Text.Json;
using System.Threading.Channels;
using EventBus.InMemory.Background;
using EventBus.Subscriptions;
using Microsoft.Extensions.Logging;

namespace EventBus.InMemory;

internal class InMemoryEventBus : IEventBus
{
    private readonly IBackgroundQueue<InternalEvent> _backgroundQueue;
    private readonly ISubscriptionCollection _subscriptionCollection;
    private readonly ILogger<InMemoryEventBus> _logger;

    public InMemoryEventBus(IBackgroundQueue<InternalEvent> backgroundQueue, 
        ISubscriptionCollection subscriptionCollection, ILogger<InMemoryEventBus> logger)
    {
        _backgroundQueue = backgroundQueue;
        _subscriptionCollection = subscriptionCollection;
        _logger = logger;
    }

    public async Task PublishAsync<TEvent>(TEvent @event, CancellationToken cancellationToken = default) where TEvent : class
    {
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

        await _backgroundQueue.EnqueueAsync(internalEvent);
        _logger.LogTrace("Published event");
    }

    public void Subscribe<TEvent, TEventHandler>() where TEvent : class where TEventHandler : IEventHandler<TEvent>
    {
        var eventName = typeof(TEvent).Name;
        var eventHandlerName = typeof(TEventHandler).Name;
        _logger.LogInformation("Subscribing to event {EventName} with {EventHandlerName}...", eventName, eventHandlerName);
        _subscriptionCollection.Add(new SubscriptionDescriptor(typeof(TEvent), typeof(TEventHandler)));
        _logger.LogInformation("Subscribed to event {EventName} with {EventHandlerName}...", eventName, eventHandlerName);
    }

    public void Unsubscribe<TEvent, TEventHandler>() where TEvent : class where TEventHandler : IEventHandler<TEvent>
    {
        var eventName = typeof(TEvent).Name;
        var eventHandlerName = typeof(TEventHandler).Name;
        _logger.LogInformation("Unsubscribing {EventHandler} from event {EventName}...",eventHandlerName ,eventName);
        _subscriptionCollection.Remove(new SubscriptionDescriptor(typeof(TEvent), typeof(TEventHandler)));
        _logger.LogInformation("Unsubscribed {EventHandler} from event {EventName}", eventHandlerName ,eventName);
    }
}