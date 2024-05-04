using Microsoft.Extensions.DependencyInjection;

namespace EventBus.Extensions.Microsoft.DependencyInjection;

public interface IEventBusBuilder
{
    /// <summary>
    /// Service collection
    /// </summary>
    public IServiceCollection Service { get; set; }
}