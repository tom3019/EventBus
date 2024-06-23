using System.Text.Encodings.Web;
using System.Text.Json;
using EventBus.Subscriptions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace EventBus.InMemory.Background;

internal class SubscriberBackgroundService: BackgroundService
{
    private readonly IBackgroundQueue<InternalEvent> _backgroundQueue;
    private readonly IServiceScopeFactory _serviceScopeFactory;

    public SubscriberBackgroundService(IBackgroundQueue<InternalEvent> backgroundQueue, 
        IServiceScopeFactory serviceScopeFactory)
    {
        _backgroundQueue = backgroundQueue;
        _serviceScopeFactory = serviceScopeFactory;
    }

    /// <summary>
    /// Executes the service
    /// </summary>
    /// <param name="stoppingToken"></param>
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        using var serviceScope = _serviceScopeFactory.CreateScope();

        var serviceProvider = serviceScope.ServiceProvider;
        var eventHandlerInvoker = serviceProvider.GetRequiredService<IEventHandlerInvoker>();
        var subscriptionCollection = serviceProvider.GetRequiredService<ISubscriptionCollection>();
        
        while (stoppingToken.IsCancellationRequested == false)
        {
            var internalEvent = await _backgroundQueue.DequeueAsync(stoppingToken);

            var eventType = subscriptionCollection
                .FirstOrDefault(x => x.EventType.Name == internalEvent.EventTypeName)?.EventType;

            if (eventType is null)
            {
                return;
            }
            
            var serializerOptions = new JsonSerializerOptions
            {
                Encoder = JavaScriptEncoder
                    .UnsafeRelaxedJsonEscaping
            };
            var @event = JsonSerializer.Deserialize(internalEvent.EventContent, eventType,serializerOptions);

            await eventHandlerInvoker.InvokeAsync(@event, stoppingToken);
        }
    }
}