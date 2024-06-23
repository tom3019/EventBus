using EventBus;
using EventBus.Extensions.Microsoft.DependencyInjection;
using EventBus.InMemory.Microsoft.DependencyInjection;
using EventBus.RabbitMq.Microsoft.DependencyInjection;
using EventBus.Sample.EventHandlers;
using EventBus.Sample.Events;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

if (builder.Environment.IsDevelopment())
{
    //builder.Services.AddScoped<IEventHandler<TestEvent>, TestEventHandler>();
    builder.Services.AddEventBus(o => o.UseSubscriptions(s => 
        s.AddEventHandlersFromAssembly()).UseInMemory());
}
else
{
    builder.Services.AddEventBus(o => o.UseSubscriptions(s => s.AddEventHandlersFromAssembly()).UseRabbitMq());
}

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

app.MapGet("/weatherforecast", async (IEventBus eventBus) =>
    {
        //eventBus.Subscribe<TestEvent, TestEventHandler>();
        var testEvent = new TestEvent
        {
            Message = "Test"
        };
        await eventBus.PublishAsync(testEvent);
        Results.Ok();
    })
    .WithName("GetWeatherForecast")
    .WithOpenApi();

app.Run();

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}