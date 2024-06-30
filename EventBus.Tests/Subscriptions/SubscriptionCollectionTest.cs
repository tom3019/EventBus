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
    public void Add_InputSubscriptionDescriptor_IfReadOnly_ThrowSubscriptionCollectionReadOnlyException()
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
    public void AddGeneric_InputSubscriptionDescriptor_IfReadOnly_ThrowSubscriptionCollectionReadOnlyException()
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
    public void Clear_IfReadOnly_ThrowSubscriptionCollectionReadOnlyException()
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
    
    [Fact]
    public void Remove_IfReadOnly_ThrowSubscriptionCollectionReadOnlyException()
    {
        // Arrange
        var subscriptionCollection = new SubscriptionCollection();
        var subscriptionDescriptor = new SubscriptionDescriptor(typeof(TestEvent), typeof(TestEventHandler));
        subscriptionCollection.Add(subscriptionDescriptor);
        subscriptionCollection.MakeReadOnly();
        
        // Act
        var action = () => subscriptionCollection.Remove(subscriptionDescriptor);

        // Assert
        action.Should().Throw<SubscriptionCollectionReadOnlyException>();
    }
    
    [Fact]
    public void Count_IfContains_ReturnCount()
    {
        // Arrange
        var subscriptionCollection = new SubscriptionCollection();
        var subscriptionDescriptor = new SubscriptionDescriptor(typeof(TestEvent), typeof(TestEventHandler));
        subscriptionCollection.Add(subscriptionDescriptor);
        
        // Act
        var result = subscriptionCollection.Count;

        // Assert
        result.Should().Be(1);
    }
    
    [Fact]
    public void Count_IfNotContains_ReturnZero()
    {
        // Arrange
        var subscriptionCollection = new SubscriptionCollection();
        
        // Act
        var result = subscriptionCollection.Count;

        // Assert
        result.Should().Be(0);
    }
    
    [Fact]
    public void GetEnumerator_IfContains_ReturnSubscriptionDescriptor()
    {
        // Arrange
        var subscriptionCollection = new SubscriptionCollection();
        var subscriptionDescriptor = new SubscriptionDescriptor(typeof(TestEvent), typeof(TestEventHandler));
        subscriptionCollection.Add(subscriptionDescriptor);
        
        // Act
        using var enumerator = subscriptionCollection.GetEnumerator();
        enumerator.MoveNext();
        var actual = enumerator.Current;

        // Assert
        actual.Should().Be(subscriptionDescriptor);
    }
    
    [Fact]
    public void GetEnumerator_IfNotContains_ReturnSubscriptionDescriptor()
    {
        // Arrange
        var subscriptionCollection = new SubscriptionCollection();
        
        // Act
        using var enumerator = subscriptionCollection.GetEnumerator();
        var actual = enumerator.Current;

        // Assert
        actual.Should().BeNull();
    }
    
    [Fact]
    public void MakeReadOnly_ReadOnlyShouldBeTrue()
    {
        // Arrange
        var subscriptionCollection = new SubscriptionCollection();
        
        // Act
        subscriptionCollection.MakeReadOnly();

        // Assert
        subscriptionCollection.IsReadOnly.Should().BeTrue();
    }
    
    [Fact]
    public void ReadOnlyDefaultShouldBeFalse()
    {
        // Arrange
        var subscriptionCollection = new SubscriptionCollection();
        
        // Assert
        subscriptionCollection.IsReadOnly.Should().BeFalse();
    }
    
    [Fact]
    public void IndexOf_IfContains_ReturnIndex()
    {
        // Arrange
        var subscriptionCollection = new SubscriptionCollection();
        var subscriptionDescriptor = new SubscriptionDescriptor(typeof(TestEvent), typeof(TestEventHandler));
        subscriptionCollection.Add(subscriptionDescriptor);
        
        // Act
        var result = subscriptionCollection.IndexOf(subscriptionDescriptor);

        // Assert
        result.Should().Be(0);
    }
    
    [Fact]
    public void IndexOf_IfNotContains_ReturnMinusOne()
    {
        // Arrange
        var subscriptionCollection = new SubscriptionCollection();
        var subscriptionDescriptor = new SubscriptionDescriptor(typeof(TestEvent), typeof(TestEventHandler));
        
        // Act
        var result = subscriptionCollection.IndexOf(subscriptionDescriptor);

        // Assert
        result.Should().Be(-1);
    }
    
    [Fact]
    public void Insert_IfReadOnly_ThrowSubscriptionCollectionReadOnlyException()
    {
        // Arrange
        var subscriptionCollection = new SubscriptionCollection();
        var subscriptionDescriptor = new SubscriptionDescriptor(typeof(TestEvent), typeof(TestEventHandler));
        subscriptionCollection.MakeReadOnly();
        
        // Act
        var action = () => subscriptionCollection.Insert(0, subscriptionDescriptor);

        // Assert
        action.Should().Throw<SubscriptionCollectionReadOnlyException>();
    }
    
    [Fact]
    public void Insert_IfNotContains_InsertSubscriptionDescriptor()
    {
        // Arrange
        var subscriptionCollection = new SubscriptionCollection();
        var subscriptionDescriptor = new SubscriptionDescriptor(typeof(TestEvent), typeof(TestEventHandler));
        
        // Act
        subscriptionCollection.Insert(0, subscriptionDescriptor);

        // Assert
        subscriptionCollection.Count.Should().Be(1);
    }
    
    [Fact]
    public void RemoveAt_IfContains_RemoveSubscriptionDescriptor()
    {
        // Arrange
        var subscriptionCollection = new SubscriptionCollection();
        var subscriptionDescriptor = new SubscriptionDescriptor(typeof(TestEvent), typeof(TestEventHandler));
        subscriptionCollection.Add(subscriptionDescriptor);
        
        // Act
        subscriptionCollection.RemoveAt(0);

        // Assert
        subscriptionCollection.Count.Should().Be(0);
    }
    
    
    [Fact]
    public void RemoveAt_IfReadOnly_ThrowSubscriptionCollectionReadOnlyException()
    {
        // Arrange
        var subscriptionCollection = new SubscriptionCollection();
        var subscriptionDescriptor = new SubscriptionDescriptor(typeof(TestEvent), typeof(TestEventHandler));
        subscriptionCollection.Add(subscriptionDescriptor);
        subscriptionCollection.MakeReadOnly();
        
        // Act
        var action = () => subscriptionCollection.RemoveAt(0);

        // Assert
        action.Should().Throw<SubscriptionCollectionReadOnlyException>();
    }
    
    [Fact]
    public void This_IfContains_ReturnSubscriptionDescriptor()
    {
        // Arrange
        var subscriptionCollection = new SubscriptionCollection();
        var subscriptionDescriptor = new SubscriptionDescriptor(typeof(TestEvent), typeof(TestEventHandler));
        subscriptionCollection.Add(subscriptionDescriptor);
        
        // Act
        var actual = subscriptionCollection[0];

        // Assert
        actual.Should().Be(subscriptionDescriptor);
    }
    
    [Fact]
    public void This_IfNotContains_ThrowArgumentOutOfRangeException()
    {
        // Arrange
        var subscriptionCollection = new SubscriptionCollection();
        
        // Act
        var action = () => subscriptionCollection[0];

        // Assert
        action.Should().Throw<ArgumentOutOfRangeException>();
    }
    
    [Fact]
    public void This_IfReadOnly_ThrowSubscriptionCollectionReadOnlyException()
    {
        // Arrange
        var subscriptionCollection = new SubscriptionCollection();
        var subscriptionDescriptor = new SubscriptionDescriptor(typeof(TestEvent), typeof(TestEventHandler));
        subscriptionCollection.MakeReadOnly();
        
        // Act
        var action = () => subscriptionCollection[0] = subscriptionDescriptor;

        // Assert
        action.Should().Throw<SubscriptionCollectionReadOnlyException>();
    }
    
    [Fact]
    public void This_ReplaceSubscription_ReturnNewSubscription()
    {
        // Arrange

        
        var subscriptionCollection = new SubscriptionCollection();
        var subscriptionDescriptor1 = new SubscriptionDescriptor(typeof(object), typeof(object));
        var subscriptionDescriptor2 = new SubscriptionDescriptor(typeof(TestEvent), typeof(TestEventHandler));
        subscriptionCollection.Add(subscriptionDescriptor1);
        
        // Act
         subscriptionCollection[0] = subscriptionDescriptor2;

        // Assert
        subscriptionCollection.First().Should().BeEquivalentTo(subscriptionDescriptor2);
        subscriptionCollection.Count.Should().Be(1);
    }
    
}