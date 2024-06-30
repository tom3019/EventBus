namespace EventBus.InMemory.Tests.TestObjects;

public class TestEventHandler : IEventHandler<TestEvent>
{
    public Task HandleAsync(TestEvent @event, CancellationToken cancellationToken = default)
    {
        return Task.CompletedTask;
    }
}