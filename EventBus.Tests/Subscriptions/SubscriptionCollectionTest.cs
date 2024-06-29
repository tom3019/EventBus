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
    public void Add_InputSubscriptionDescriptor_IfContains_NotAdd()
    {
        // Arrange
        var subscriptionCollection = new SubscriptionCollection();
        var subscriptionDescriptor1 = new SubscriptionDescriptor(typeof(TestEvent), typeof(TestEventHandler));
        subscriptionCollection.Add(subscriptionDescriptor1);
        
        var subscriptionDescriptor2 = new SubscriptionDescriptor(typeof(TestEvent), typeof(TestEventHandler));

        // Act
        subscriptionCollection.Add(subscriptionDescriptor2);

        // Assert
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
    public void AddGeneric_InputSubscriptionDescriptor_IfContains_NotAdd()
    {
        // Arrange
        var subscriptionCollection = new SubscriptionCollection();
        subscriptionCollection.Add<TestEvent,TestEventHandler>();
        
        // Act
        subscriptionCollection.Add<TestEvent,TestEventHandler>();

        // Assert
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
    
    [Fact]
    public void Contains_IfContains_ReturnTrue()
    {
        // Arrange
        var subscriptionCollection = new SubscriptionCollection();
        var subscriptionDescriptor = new SubscriptionDescriptor(typeof(TestEvent), typeof(TestEventHandler));
        subscriptionCollection.Add(subscriptionDescriptor);
        
        // Act
        var result = subscriptionCollection.Contains(subscriptionDescriptor);

        // Assert
        result.Should().BeTrue();
    }
    
    [Fact]
    public void Contains_IfNotContains_ReturnFalse()
    {
        // Arrange
        var subscriptionCollection = new SubscriptionCollection();
        var subscriptionDescriptor = new SubscriptionDescriptor(typeof(TestEvent), typeof(TestEventHandler));
        
        // Act
        var result = subscriptionCollection.Contains(subscriptionDescriptor);

        // Assert
        result.Should().BeFalse();
    }
    
    [Fact]
    
    public void CopyTo_CopyToSubscriptionDescriptorArray()
    {
        // Arrange
        var subscriptionCollection = new SubscriptionCollection();
        var subscriptionDescriptor = new SubscriptionDescriptor(typeof(TestEvent), typeof(TestEventHandler));
        subscriptionCollection.Add(subscriptionDescriptor);
        
        var array = new SubscriptionDescriptor[1];
        
        // Act
        subscriptionCollection.CopyTo(array, 0);

        // Assert
        array[0].Should().BeEquivalentTo(subscriptionDescriptor);
    }
    
    [Fact]
    public void Remove_IfContains_RemoveSubscriptionDescriptor()
    {
        // Arrange
        var subscriptionCollection = new SubscriptionCollection();
        var subscriptionDescriptor = new SubscriptionDescriptor(typeof(TestEvent), typeof(TestEventHandler));
        subscriptionCollection.Add(subscriptionDescriptor);
        
        // Act
        var result = subscriptionCollection.Remove(subscriptionDescriptor);

        // Assert
        result.Should().BeTrue();
        subscriptionCollection.Count.Should().Be(0);
    }
    
    [Fact]
    public void Remove_IfNotContains_NotRemoveSubscriptionDescriptor()
    {
        // Arrange
        var subscriptionCollection = new SubscriptionCollection();
        var subscriptionDescriptor = new SubscriptionDescriptor(typeof(TestEvent), typeof(TestEventHandler));
        
        // Act
        var result = subscriptionCollection.Remove(subscriptionDescriptor);

        // Assert
        result.Should().BeFalse();
        subscriptionCollection.Count.Should().Be(0);
    }
}