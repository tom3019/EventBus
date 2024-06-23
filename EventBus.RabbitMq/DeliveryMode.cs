namespace EventBus.RabbitMq;

internal enum DeliveryMode : byte
{
    /// <summary>
    ///     The non-persistent
    /// </summary>
    NonPersistent = 1,

    /// <summary>
    ///     The persistent
    /// </summary>
    Persistent = 2
}