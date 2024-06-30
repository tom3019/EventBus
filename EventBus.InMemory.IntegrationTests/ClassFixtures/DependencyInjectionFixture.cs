using EventBus.Extensions.Microsoft.DependencyInjection;
using EventBus.InMemory.Microsoft.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace EventBus.InMemory.IntegrationTests.ClassFixtures;

public class DependencyInjectionFixture : IDisposable
{
    public IServiceProvider ServiceProvider { get; private set; }
    
    public DependencyInjectionFixture()
    {
        var serviceCollection = new ServiceCollection();
        serviceCollection.AddEventBus(b => ServiceCollectionExtensions.UseInMemory(b).UseSubscriptions());
        serviceCollection.AddLogging(l => l.AddConsole());
        ServiceProvider = serviceCollection.BuildServiceProvider();
    }

    public void Dispose()
    {
    }
}