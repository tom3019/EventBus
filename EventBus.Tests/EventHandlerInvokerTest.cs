using EventBus.Subscriptions;
using EventBus.Tests.TestObjects;
using FluentAssertions;
using NSubstitute;

namespace EventBus.Tests;

public class EventHandlerInvokerTest
{
    private readonly ISubscriptionProvider _subscriptionProvider;
    
    public EventHandlerInvokerTest()
    {
        _subscriptionProvider = Substitute.For<ISubscriptionProvider>();
    }

    private IEventHandlerInvoker GetSystemUnderTest()
    {
        return new EventHandlerInvoker(_subscriptionProvider);
    }
    
    [Fact]
    public async Task InvokeAsyncGeneric_WhenEventIsNull_ThrowsArgumentNullException()
    {
        // Arrange
        // Act
        var sut = GetSystemUnderTest();
        var act = async () => await sut.InvokeAsync<object>(null);
        
        // Assert
        await act.Should().ThrowAsync<ArgumentNullException>();
    }
    
    [Fact]
    public async Task InvokeAsyncGeneric_WhenEventIsNotNull_InvokeEventHandlers()
    {
        // Arrange
        var @event = new TestEvent();
        var eventHandlers = new List<IEventHandler<TestEvent>>
        {
            Substitute.For<TestEventHandler>()
        };
        _subscriptionProvider.GetEventHandlers<TestEvent>().Returns(eventHandlers);
        
        // Act
        var sut = GetSystemUnderTest();
        await sut.InvokeAsync(@event);
        
        // Assert
        eventHandlers.ForEach(eventHandler => eventHandler.Received(1)
            .HandleAsync(@event));
    }
    
    [Fact]
    public async Task InvokeAsync_WhenEventIsNull_ThrowsArgumentNullException()
    {
        // Arrange
        // Act
        var sut = GetSystemUnderTest();
        var act = async () => await sut.InvokeAsync(null);
        
        // Assert
        await act.Should().ThrowAsync<ArgumentNullException>();
    }
    
    [Fact]
    public async Task InvokeAsync_WhenEventIsNotNull_InvokeEventHandlers()
    {
        // Arrange
        var @event = new TestEvent();
        var eventHandlers = new List<IEventHandler<TestEvent>>
        {
            Substitute.For<TestEventHandler>()
        };
        _subscriptionProvider.GetEventHandlers(@event).Returns(eventHandlers);
        
        // Act
        var sut = GetSystemUnderTest();
        await sut.InvokeAsync((object)@event);
        
        // Assert
        eventHandlers.ForEach(eventHandler => eventHandler.Received(1)
            .HandleAsync(@event));
    }
}