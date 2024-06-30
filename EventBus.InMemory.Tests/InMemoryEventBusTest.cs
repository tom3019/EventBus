using EventBus.InMemory.Background;
using EventBus.InMemory.Tests.TestObjects;
using EventBus.Subscriptions;
using Microsoft.Extensions.Logging;
using NSubstitute;

namespace EventBus.InMemory.Tests;

public class InMemoryEventBusTest
{
    private readonly IBackgroundQueue<InternalEvent> _backgroundQueue;
    private readonly ISubscriptionCollection _subscriptionCollection;
    private readonly ILogger<InMemoryEventBus> _logger;

    public InMemoryEventBusTest()
    {
        _backgroundQueue = Substitute.For<IBackgroundQueue<InternalEvent>>();
        _subscriptionCollection = Substitute.For<ISubscriptionCollection>();
        _logger = Substitute.For<ILogger<InMemoryEventBus>>();
    }

    private IEventBus GetSystemUnderTest()
    {
        return new InMemoryEventBus(_backgroundQueue, _subscriptionCollection, _logger);
    }

    [Fact]
    public async Task PublishAsync_ShouldEnqueueEvent()
    {
        // Arrange
        var eventBus = GetSystemUnderTest();
        var @event = new TestEvent();

        // Act
        await eventBus.PublishAsync(@event);

        // Assert
        await _backgroundQueue.Received(1).EnqueueAsync(Arg.Any<InternalEvent>());
    }

    [Fact]
    public void Subscribe_ShouldAddSubscription()
    {
        // Arrange
        var eventBus = GetSystemUnderTest();

        // Act
        eventBus.Subscribe<TestEvent, TestEventHandler>();

        // Assert
        _subscriptionCollection.Received(1).Add(Arg.Any<SubscriptionDescriptor>());
    }
    
    [Fact]
    public void Unsubscribe_ShouldRemoveSubscription()
    {
        // Arrange
        var eventBus = GetSystemUnderTest();

        // Act
        eventBus.Unsubscribe<TestEvent, TestEventHandler>();

        // Assert
        _subscriptionCollection.Received(1).Remove(Arg.Any<SubscriptionDescriptor>());
    }
}