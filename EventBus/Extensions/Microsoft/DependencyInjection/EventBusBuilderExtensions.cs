using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace EventBus.Extensions.Microsoft.DependencyInjection;

/// <summary>
/// EventBusBuilderExtensions
/// </summary>
public static class EventBusBuilderExtensions
{
    /// <summary>
    /// add event handlers from assembly
    /// </summary>
    /// <param name="eventBusBuilder"></param>
    /// <param name="eventHandlerServiceLifetime"></param>
    /// <returns></returns>
    public static IEventBusBuilder AddEventHandlersFromAssembly(this IEventBusBuilder eventBusBuilder,
        ServiceLifetime eventHandlerServiceLifetime = ServiceLifetime.Scoped)
    {
        var entryAssembly = Assembly.GetEntryAssembly();
        if (entryAssembly is null)
        {
            return eventBusBuilder;
        }

        var eventHandlerDescriptors = entryAssembly.GetExportedTypes().Distinct()
            .Where(t => !t.IsAbstract)
            .Where(t => !t.IsGenericTypeDefinition)
            .Where(t => t.GetInterfaces()
                .Any(c => c.GetGenericTypeDefinition() == typeof(IEventHandler<>)))
            .Select(handlerType => new
            {
                InterfaceTypes = handlerType.GetInterfaces()
                    .Where(c => c.GetGenericTypeDefinition() == typeof(IEventHandler<>)),
                HandlerType = handlerType
            }).Where(w=>w.InterfaceTypes.Any())
            .ToList();

        foreach (var descriptor in eventHandlerDescriptors)
        {
            foreach (var interfaceType in descriptor.InterfaceTypes)
            {
                eventBusBuilder.Service.TryAdd(
                    new ServiceDescriptor(
                        interfaceType,
                        descriptor.HandlerType,
                        eventHandlerServiceLifetime
                    ));
            }
        }

        return eventBusBuilder;
    }
    
}