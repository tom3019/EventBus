namespace EventBus.RabbitMq.Attributes;

[AttributeUsage(AttributeTargets.Class)]
public class ExchangeAttribute : Attribute
{
    /// <summary>
    /// Gets or sets the name of the exchange.
    /// </summary>
    public string ExchangeName { get; set; }
    
    /// <summary>
    /// Gets or sets the type of the exchange.
    /// </summary>
    public string ExchangeType { get; set; }
}