using EventBus.Subscriptions;
using EventBus.Subscriptions.Exceptions;
using EventBus.Tests.TestObjects;
using FluentAssertions;

namespace EventBus.Tests.Subscriptions;

public class SubscriptionCollectionTest
{
    [Fact]
    public void Add_InputSubscriptionDescriptor_AddedSubscriptionDescriptor()
    {
        // Arrange
        var subscriptionCollection = new SubscriptionCollection();
        var subscriptionDescriptor = new SubscriptionDescriptor(typeof(TestEvent), typeof(TestEventHandler));

        // Act
        subscriptionCollection.Add(subscriptionDescriptor);

        // Assert
        subscriptionCollection.First().Should().BeEquivalentTo(subscriptionDescriptor);
        subscriptionCollection.Count.Should().Be(1);
    }

    [Fact]
    public void Add_InputSubscriptionDescriptor_IfReadOnly_ThrowReadOnlyException()
    {
        // Arrange
        var subscriptionCollection = new SubscriptionCollection();
        subscriptionCollection.MakeReadOnly();

        var subscriptionDescriptor = new SubscriptionDescriptor(typeof(TestEvent), typeof(TestEventHandler));

        // Act
        var action = () => subscriptionCollection.Add(subscriptionDescriptor);

        // Assert
        action.Should().Throw<SubscriptionCollectionReadOnlyException>();
    }
    
    [Fact]
    public void AddGeneric_InputSubscriptionDescriptor_AddedSubscriptionDescriptor()
    {
        // Arrange
        var subscriptionCollection = new SubscriptionCollection();
        var expected = new SubscriptionDescriptor(typeof(TestEvent), typeof(TestEventHandler));

        // Act
        subscriptionCollection.Add<TestEvent,TestEventHandler>();

        // Assert
        subscriptionCollection.First().Should().BeEquivalentTo(expected);
        subscriptionCollection.Count.Should().Be(1);
    }
    
    [Fact]
    public void AddGeneric_InputSubscriptionDescriptor_IfReadOnly_ThrowReadOnlyException()
    {
        // Arrange
        var subscriptionCollection = new SubscriptionCollection();
        subscriptionCollection.MakeReadOnly();
        
        // Act
        var action = () => subscriptionCollection.Add<TestEvent,TestEventHandler>();

        // Assert
        action.Should().Throw<SubscriptionCollectionReadOnlyException>();
    }
    
    [Fact]
    public void Clear_IfReadOnly_ThrowReadOnlyException()
    {
        // Arrange
        var subscriptionCollection = new SubscriptionCollection();
        subscriptionCollection.MakeReadOnly();
        
        // Act
        var action = () => subscriptionCollection.Clear();

        // Assert
        action.Should().Throw<SubscriptionCollectionReadOnlyException>();
    }

    [Fact]
    public void Clear_CollectionCountShouldBeZero()
    {
        // Arrange
        var subscriptionCollection = new SubscriptionCollection();
        subscriptionCollection.Add<TestEvent,TestEventHandler>();
        
        // Act
        subscriptionCollection.Clear();
        
        // Assert
        subscriptionCollection.Count.Should().Be(0);
    }
}