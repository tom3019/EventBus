using EventBus.Extensions.Microsoft.DependencyInjection;
using EventBus.Subscriptions;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;

namespace EventBus.Tests.Extensions.Microsoft.DependencyInjection;

public class EventBusServiceCollectionExtensionsTest
{
    private readonly IServiceProvider _serviceProvider;
    public EventBusServiceCollectionExtensionsTest()
    {
        // Arrange
        var serviceCollection = new ServiceCollection();
        serviceCollection.AddEventBus(_ => { });
        var serviceScope =  serviceCollection.BuildServiceProvider().CreateScope();
        _serviceProvider = serviceScope.ServiceProvider;
    }
    
    [Fact]
    public void AddEventBus_InputServiceCollectionAndSetup_AddedEventBus()
    {
        // Act
        var subscriptionProvider = _serviceProvider.GetRequiredService<ISubscriptionProvider>();
        var eventHandlerInvoker = _serviceProvider.GetRequiredService<IEventHandlerInvoker>();

        // Assert
        subscriptionProvider.GetType().Should().Be(typeof(SubscriptionProvider));
        eventHandlerInvoker.GetType().Should().Be(typeof(EventHandlerInvoker));
    }
}