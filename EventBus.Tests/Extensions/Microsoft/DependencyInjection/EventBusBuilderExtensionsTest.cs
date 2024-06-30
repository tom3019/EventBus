using EventBus.Extensions.Microsoft.DependencyInjection;
using EventBus.Subscriptions;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;

namespace EventBus.Tests.Extensions.Microsoft.DependencyInjection;

public class EventBusBuilderExtensionsTest
{
    [Fact]
    public void UseSubscriptions_InputEventBusBuilder_AddedSubscriptionCollection()
    {
        // Arrange
        var serviceCollection = new ServiceCollection();
        serviceCollection.AddEventBus(b => b.UseSubscriptions());
        var serviceScope =  serviceCollection.BuildServiceProvider().CreateScope();
        var serviceProvider = serviceScope.ServiceProvider;
        
        // Act
        var subscriptionCollection = serviceProvider.GetRequiredService<ISubscriptionCollection>();

        // Assert
        subscriptionCollection.GetType().Should().Be(typeof(SubscriptionCollection));
    }
}