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
    /// add event handlers from assemblies
    /// </summary>
    /// <param name="eventBusBuilder"></param>
    /// <param name="eventHandlerServiceLifetime"></param>
    /// <returns></returns>
    public static IEventBusBuilder AddEventHandlersFromAssemblies(this IEventBusBuilder eventBusBuilder,
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
            .Where(t=> t.GetInterfaces()
                .Any(c => c.GetGenericTypeDefinition() == typeof(IEventHandler<>)))
            .Select(handlerType =>new
            {
                InterfaceType = handlerType.GetInterfaces()
                    .FirstOrDefault(c => c.GetGenericTypeDefinition() == typeof(IEventHandler<>)),
                HandlerType = handlerType
            }).Where(x=>x.InterfaceType != null).ToList();

        foreach (var descriptor in eventHandlerDescriptors)
        {
            eventBusBuilder.Service.TryAdd(
                new ServiceDescriptor(
                    descriptor.InterfaceType!,
                    descriptor.HandlerType,
                    eventHandlerServiceLifetime
                ));
        }


        return eventBusBuilder;
    }
}