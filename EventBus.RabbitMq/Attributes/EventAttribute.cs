namespace EventBus.RabbitMq.Attributes;

[AttributeUsage(AttributeTargets.Class, Inherited = false)]
public class EventAttribute: Attribute
{
    /// <summary>
    ///     Initializes a new instance of the EventAttribute class.
    /// </summary>
    /// <param name="routingKey">The name of the event.</param>
    /// <param name="priority">The priority of the event.</param>
    /// <param name="version">The version of the event as a string (e.g., "1.0.0").</param>
    public EventAttribute(string routingKey, byte priority = 0, string version = "1.0.0")
    {
        RoutingKey = routingKey;
        Priority = priority;
        Version = new Version(version);
    }
        
    /// <summary>
    ///     Gets the name of the event.
    /// </summary>
    public string RoutingKey { get; set; }

    /// <summary>
    ///     Gets the priority of the event, which can be used for message prioritization in the queue.
    /// </summary>
    public byte Priority { get; set; }

    /// <summary>
    ///     Gets the version of the event, which can be used for versioning purposes.
    /// </summary>
    public Version Version { get; }
}