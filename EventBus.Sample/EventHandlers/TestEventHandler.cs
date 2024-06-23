using EventBus.Sample.Events;

namespace EventBus.Sample.EventHandlers;

public class TestEventHandler : IEventHandler<TestEvent>
{
    public Task HandleAsync(TestEvent @event, CancellationToken cancellationToken = default)
    {
        Console.WriteLine("接收到Event");
        return Task.CompletedTask;
    }
}