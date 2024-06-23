using Microsoft.Extensions.DependencyInjection;

namespace EventBus.Extensions.Microsoft.DependencyInjection;

public interface ISubscriptionBuilder
{
    IServiceCollection ServiceCollection { get; set; }
}