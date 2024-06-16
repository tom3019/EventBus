using System.Reflection;
using EventBus.Subscriptions;
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
                .Any(x => x.IsGenericType && x.GetGenericTypeDefinition() == typeof(IEventHandler<>)))
            .Select(handlerType =>
            {
                var interfaces = handlerType.GetInterfaces()
                    .Where(c => c.GetGenericTypeDefinition() == typeof(IEventHandler<>));
                
                return new
                {
                    InterfaceTypes = interfaces,
                    EventType = interfaces.FirstOrDefault().GetGenericArguments().FirstOrDefault(),
                    HandlerType = handlerType
                };
            }).Where(w=>w.InterfaceTypes.Any())
            .ToList();

        eventBusBuilder.ServiceCollection.TryAddSingleton<ISubscriptionCollection>(provider =>
        {
            var subscriptionCollection = new SubscriptionCollection();
            foreach (var descriptor in eventHandlerDescriptors)
            {
                subscriptionCollection.Add(
                    new SubscriptionDescriptor(descriptor.EventType, descriptor.HandlerType));
            }
        
            return subscriptionCollection;
        });
        
        foreach (var descriptor in eventHandlerDescriptors)
        {
            foreach (var interfaceType in descriptor.InterfaceTypes)
            {
                eventBusBuilder.ServiceCollection.TryAdd(
                    new ServiceDescriptor(
                        interfaceType,
                        descriptor.HandlerType,
                        eventHandlerServiceLifetime
                    ));
            }
        }
        
        eventBusBuilder.ServiceCollection.AddScoped<ISubscriptionProvider, SubscriptionProvider>();
        
        return eventBusBuilder;
    }
    
}