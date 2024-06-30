using EventBus.InMemory.Background;
using EventBus.InMemory.IntegrationTests.ClassFixtures;
using EventBus.InMemory.IntegrationTests.TestObjects;
using EventBus.Subscriptions;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace EventBus.InMemory.IntegrationTests;

public class InMemoryEventBusTest : IClassFixture<DependencyInjectionFixture>
{
    private readonly DependencyInjectionFixture _fixture;
    public InMemoryEventBusTest(DependencyInjectionFixture fixture)
    {
        _fixture = fixture;
    }
    
    private IEventBus GetSystemUnderTest()
    {
        return _fixture.ServiceProvider.GetRequiredService<IEventBus>();
    }

    [Fact]
    public async Task PublishAsync_WhenCalled_ShouldPublishEvent()
    {
        // Arrange
        var @event = new TestEvent();
        var eventBus = GetSystemUnderTest();

        // Act
        await eventBus.PublishAsync(@event);

        // Assert
        var backgroundQueue = _fixture.ServiceProvider.GetRequiredService<IBackgroundQueue<InternalEvent>>();
        var internalEvent = await backgroundQueue.DequeueAsync(CancellationToken.None);
        internalEvent.Should().NotBeNull();
        internalEvent.EventTypeName.Should().Be(@event.GetType().Name);
        internalEvent.EventContent.Should().NotBeNullOrEmpty();
    }

    [Fact]
    public void Subscribe_WhenCalled_ShouldSubscribeEvent()
    {
        // Arrange
        var eventBus = GetSystemUnderTest();


        // Act
        eventBus.Subscribe<TestEvent, IEventHandler<TestEvent>>();

        // Assert
        var subscriptionCollection = _fixture.ServiceProvider.GetRequiredService<ISubscriptionCollection>();
        subscriptionCollection.Count.Should().Be(1);
    }
    
    [Fact]
    public void Unsubscribe_WhenCalled_ShouldUnsubscribeEvent()
    {
        // Arrange
        var eventBus = GetSystemUnderTest();
        eventBus.Subscribe<TestEvent, IEventHandler<TestEvent>>();

        // Act
        eventBus.Unsubscribe<TestEvent, IEventHandler<TestEvent>>();

        // Assert
        var subscriptionCollection = _fixture.ServiceProvider.GetRequiredService<ISubscriptionCollection>();
        subscriptionCollection.Count.Should().Be(0);
    }

    [Fact]
    public async Task PublishAsync_WhenCalled_ShouldInvokeEventHandler()
    {
        // Arrange
        var @event = new TestEvent();
        var eventBus = GetSystemUnderTest();
        eventBus.Subscribe<TestEvent, IEventHandler<TestEvent>>();
        var backgroundService = _fixture.ServiceProvider.GetRequiredService<IHostedService>();
        await backgroundService.StartAsync(CancellationToken.None);

        // Act
        await eventBus.PublishAsync(@event);
        await backgroundService.StopAsync(CancellationToken.None);
        
        // Assert
        var backgroundQueue = _fixture.ServiceProvider.GetRequiredService<IBackgroundQueue<InternalEvent>>();
        backgroundQueue.GetQueueCount().Should().Be(0);
    }
}