namespace EventBus.RabbitMq;

public class RabbitMqOption
{
      public RabbitMqOption()
        {
            this.ClientProvidedName = ApplicationInformation.ManifestModuleName;
            this.ApplicationName = ApplicationInformation.ApplicationName;
            
            this.ExchangeName = ApplicationInformation.ApplicationName.ToLower();
            this.QueueName = ApplicationInformation.ApplicationName.ToLower();
            
            this.ConnectionString = "localhost";
            this.VirtualHost = "/";
            this.UserName = "guest";
            this.Password = "guest";
            this.PublishRetryCount = 5;
            this.SubscribeRetryTime = 5;
            this.Durable = true;
            this.AutoDelete = false;
        }

        /// <summary>
        ///     Gets or sets the value of the client provided name
        /// </summary>
        public string ClientProvidedName { get; set; }

        /// <summary>
        /// Gets or sets the value of the application name
        /// </summary>
        public string ApplicationName { get; set; }

        /// <summary>
        ///     Gets or sets the value of the exchange name
        /// </summary>
        public string ExchangeName { get; set; }

        /// <summary>
        ///     Gets or sets the value of the queue name
        /// </summary>
        public string QueueName { get; set; }

        /// <summary>
        ///     Gets or sets the value of the connection string
        /// </summary>
        public string ConnectionString { get; set; } 

        /// <summary>
        ///     Gets or sets the value of the virtual host
        /// </summary>
        public string VirtualHost { get; set; } 

        /// <summary>
        ///     Gets or sets the value of the user name
        /// </summary>
        public string UserName { get; set; } 

        /// <summary>
        ///     Gets or sets the value of the password
        /// </summary>
        public string Password { get; set; } 

        /// <summary>
        ///     Gets or sets the value of the publish retry count
        /// </summary>
        public int PublishRetryCount { get; set; } 

        /// <summary>
        ///     Gets or sets the value of the subscribe retry time
        /// </summary>
        public int SubscribeRetryTime { get; set; }


        /// <summary>
        /// Gets or sets the value of the durable
        /// </summary>
        public bool Durable { get; set; } 

        /// <summary>
        /// Gets or sets the value of the auto delete
        /// </summary>
        public bool AutoDelete { get; set; } 

        /// <summary>
        /// Gets or sets the value of the max priority
        /// </summary>
        public byte MaxPriority { get; set; } = default;

        /// <summary>
        /// Gets or sets the value of the consume prefetch count
        /// </summary>
        public ushort ConsumePrefetchCount { get; set; } = default;
}