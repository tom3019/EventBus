using EventBus.Subscriptions;
using EventBus.Tests.ClassFixtures;
using EventBus.Tests.TestObjects;
using FluentAssertions;

namespace EventBus.Tests.Subscriptions;

public class SubscriptionProviderTest : IClassFixture<DependencyInjectionFixture>
{
    private readonly DependencyInjectionFixture _fixture;
    public SubscriptionProviderTest(DependencyInjectionFixture fixture)
    {
        _fixture = fixture;
    }


    private ISubscriptionProvider GetSystemUnderTest()
    {
        return new SubscriptionProvider(_fixture.ServiceProvider);
    }
    
    [Fact]
    public void GetEventHandlersGeneric_InputEvent_ReturnEventHandlers()
    {
        // Arrange

        // Act
        var sut = GetSystemUnderTest();
        var actual = sut.GetEventHandlers<TestEvent>();

        // Assert
        actual.First().GetType().Should().Be(typeof(TestEventHandler));
        actual.Should().HaveCount(1);
    }
    
    [Fact]
    public void GetEventHandlers_InputEvent_ReturnObject()
    {
        // Arrange
        var sut = GetSystemUnderTest();
        var @event = new TestEvent();

        // Act
        var actual = sut.GetEventHandlers(@event);

        // Assert
        actual.First().GetType().Should().Be(typeof(TestEventHandler));
    }
}