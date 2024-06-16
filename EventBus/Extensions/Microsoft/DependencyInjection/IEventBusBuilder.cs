using EventBus.Subscriptions;
using Microsoft.Extensions.DependencyInjection;

namespace EventBus.Extensions.Microsoft.DependencyInjection;

/// <summary>
/// event bus builder interface
/// </summary>
public interface IEventBusBuilder
{
    /// <summary>
    /// Service collection
    /// </summary>
    public IServiceCollection ServiceCollection { get; }
}