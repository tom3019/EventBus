namespace EventBus.InMemory.Background;

internal class InternalEvent
{
    /// <summary>
    /// Gets or sets the value of the id
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Gets or sets the value of the event type name
    /// </summary>
    public string EventTypeName { get; set; }

    /// <summary>
    /// Gets or sets the value of the event content
    /// </summary>
    public string EventContent { get; set; }

    /// <summary>
    /// Gets or sets the value of the create at
    /// </summary>
    public DateTime CreateAt { get; set; }
}