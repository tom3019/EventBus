namespace EventBus.InMemory.Background;

public interface IBackgroundQueue<T>
{
    ValueTask EnqueueAsync(T workItem);
    
    ValueTask<T> DequeueAsync(CancellationToken cancellationToken);

    int GetQueueCount();
}