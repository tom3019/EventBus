using Microsoft.Extensions.DependencyInjection;

namespace EventBus.Extensions.Microsoft.DependencyInjection;

public class EventBusBuilder:IEventBusBuilder
{
    public IServiceCollection Service { get; set; }

    public EventBusBuilder(IServiceCollection service)
    {
        Service = service;
    }
}