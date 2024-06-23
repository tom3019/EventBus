namespace EventBus.RabbitMq;

internal class AmqpEndPoint
{
    /// <summary>
    /// Gets the value of the host
    /// </summary>
    public string Host { get; }

    /// <summary>
    /// Gets the value of the port
    /// </summary>
    public ushort Port { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="AmqpEndPoint"/> class
    /// </summary>
    /// <param name="host">The host</param>
    /// <param name="port">The port</param>
    public AmqpEndPoint(string host,ushort port)
    {
        this.Host = host;
        this.Port = port;
    }
}