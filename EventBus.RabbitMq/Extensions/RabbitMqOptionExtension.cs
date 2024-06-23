using EasyNetQ;

namespace EventBus.RabbitMq.Extensions;

internal static class RabbitMqOptionExtension
{
    private static IEnumerable<AmqpPoint> GetConnectionStrings(this RabbitMqOption rabbitMqOption)
    {
        var endpoints = rabbitMqOption.ConnectionString.Split(separator: ',');
        var amqpPoints = new List<AmqpPoint>();
        foreach (var endpoint in endpoints)
        {
            var url = endpoint.Split(separator: ':');
            var hostName = url.ElementAt(0);
            var portParse = ushort.TryParse(url.ElementAt(1), out var port);
            amqpPoints.Add(new AmqpPoint(hostName, portParse ? port : (ushort)5672));
        }

        return amqpPoints;
    }
    
    /// <summary>
    /// Returns the connection configuration using the specified rabbit mq option
    /// </summary>
    /// <param name="rabbitMqOption">The rabbit mq option</param>
    /// <returns>The connection configuration</returns>
    internal static ConnectionConfiguration ToConnectionConfiguration(this RabbitMqOption rabbitMqOption)
    {
        var endPoints = rabbitMqOption.GetConnectionStrings().Select(x => new HostConfiguration
        {
            Host = x.Host,
            Port = x.Port
        }).ToList();

        return new ConnectionConfiguration
        {
            UserName = rabbitMqOption.UserName,
            Password = rabbitMqOption.Password,
            Hosts = endPoints,
            VirtualHost = rabbitMqOption.VirtualHost,
            Name = rabbitMqOption.ClientProvidedName
        };
    }
}