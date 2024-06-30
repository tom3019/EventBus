using EventBus.Tests.TestObjects;
using Microsoft.Extensions.DependencyInjection;

namespace EventBus.Tests.ClassFixtures;

public class DependencyInjectionFixture : IDisposable
{
    public IServiceProvider ServiceProvider { get; private set; }

    public DependencyInjectionFixture()
    {
        var serviceCollection = new ServiceCollection();
        serviceCollection.AddSingleton<IEventHandler<TestEvent>, TestEventHandler>();
        ServiceProvider = serviceCollection.BuildServiceProvider();
    }
    public void Dispose()
    {
    }
}