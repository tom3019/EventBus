namespace EventBus.RabbitMq.Attributes;

[AttributeUsage(AttributeTargets.Class)]
public class QueueAttribute : Attribute
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="QueueAttribute" /> class with the specified queue settings.
    /// </summary>
    /// <param name="queueName">The name of the queue.</param>
    /// <param name="durable">Specifies whether the queue should be durable. Default is true.</param>
    /// <param name="exclusive">Specifies whether the queue should be exclusive. Default is false.</param>
    /// <param name="autoDelete">Specifies whether the queue should be auto-deleted. Default is false.</param>
    public QueueAttribute(string queueName, bool durable = true, bool exclusive = false, bool autoDelete = false)
    {
        QueueName = queueName;
        Durable = durable;
        Exclusive = exclusive;
        AutoDelete = autoDelete;
    }

    /// <summary>
    ///     Gets the name of the queue.
    /// </summary>
    public string QueueName { get; set; }

    /// <summary>
    ///     Gets a value indicating whether the queue is durable.
    /// </summary>
    public bool Durable { get; set; }

    /// <summary>
    ///     Gets a value indicating whether the queue is exclusive.
    /// </summary>
    public bool Exclusive { get; set; }

    /// <summary>
    ///     Gets a value indicating whether the queue should be auto-deleted.
    /// </summary>
    public bool AutoDelete { get; set; }
}