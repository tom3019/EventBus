using System.Threading.Channels;

namespace EventBus.InMemory.Background;

public class BackgroundQueue<T> : IBackgroundQueue<T> where T : class
{
    private readonly Channel<T> _workItems;

    public BackgroundQueue(int capacity)
    {
        var options = new BoundedChannelOptions(capacity)
        {
            FullMode = BoundedChannelFullMode.Wait
        };

        _workItems = Channel.CreateBounded<T>(options);
    }

    public ValueTask EnqueueAsync(T workItem)
    {
        if (workItem is null)
        {
            throw new ArgumentNullException(nameof(workItem));
        }

        return _workItems.Writer.WriteAsync(workItem);
    }

    public ValueTask<T> DequeueAsync(CancellationToken cancellationToken)
    {
        return _workItems.Reader.ReadAsync(cancellationToken);
    }
}