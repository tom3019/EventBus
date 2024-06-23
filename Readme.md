# EventBus
## Overview

此套件實現了事件總線(EventBus)的功能，
可以讓不同的物件之間透過事件來溝通。

### Install Packages
#### Core Library
```shell
Install-Package EventBus -Version 0.0.1-beta
```

#### InMemory EventBus
```shell
Install-Package EventBus.InMemory -Version 0.0.1-beta
```

#### RabbitMQ EventBus
```shell
Install-Package EventBus.RabbitMQ -Version 0.0.1-beta
```

## Sample Usage
### Dependency Injection
#### InMemory EventBus
program.cs
```csharp
builder.Services.AddEventBus(o => o.AddEventHandlersFromAssembly().UseInMemory());
```

#### RabbitMQ EventBus
program.cs
```csharp
builder.Services.AddEventBus(o => o.AddEventHandlersFromAssembly().UseRabbitMQ());
```
appsettings.json
```json
{
  "RabbitMqOption": {
    "ExchangeName": "sample.exchange",
    "QueueName": "sample.queue",
    "ConnectionString": "localhost:5672",
    "UserName": "user",
    "Password": "password",
    "VirtualHost": "/",
    "PublishRetryCount": 3,
    "SubscribeRetryTime": 3,
    "MaxPriority" : 10,
    "ConsumePrefetchCount": 2
  }
}
```
##### OR
```csharp
builder.Services.AddEventBus(o => o.AddEventHandlersFromAssembly().UseRabbitMQ(o=>
{
    //options
}));
```

### Publish Event
使用`IEventBus`的`PublishAsync`方法來發佈事件。
```csharp
app.MapGet("/weatherforecast", async (IEventBus eventBus) =>
    {
        var testEvent = new TestEvent
        {
            Message = "Test"
        };
        await eventBus.PublishAsync(testEvent);
        Results.Ok();
    })
    .WithName("GetWeatherForecast")
    .WithOpenApi();
```

### Subscribe Event
#### Auto Register
實現`IEventHandler<TEvent>`接口，並會在`AddEventHandlersFromAssembly`方法中自動註冊訂閱。
```csharp

public class TestEventHandler : IEventHandler<TestEvent>
{
    public Task HandleAsync(TestEvent @event)
    {
        Console.WriteLine(@event.Message);
        return Task.CompletedTask;
    }
}
```

#### Use `SubscribeAsync` Method
```csharp
app.MapGet("/weatherforecast", async (IEventBus eventBus) =>
    {
        await eventBus.SubscribeAsync<TestEvent>(async @event =>
        {
            Console.WriteLine(@event.Message);
        });
        Results.Ok();
    })
    .WithName("GetWeatherForecast")
    .WithOpenApi();
```