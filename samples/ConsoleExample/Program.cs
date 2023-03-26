using System.Diagnostics;
using ConsoleExample.Notifications;
using MediatR;
using MediatR.ParallelPublisher;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

var terminationTokenSource = new CancellationTokenSource();

Console.CancelKeyPress += (_, _) => terminationTokenSource.Cancel();

using var host = new HostBuilder()
    .ConfigureServices((_, services) =>
    {
        services.AddLogging(builder =>
        {
            builder.AddConsole();
        });
        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssemblyContaining<MyNormalNotification>();
            cfg.UseParallelNotificationPublisher(services);
        });
    })
    .Build();

//run the host
await host.StartAsync(terminationTokenSource.Token);

var mediator = host.Services.GetRequiredService<IMediator>();
var logger = host.Services.GetRequiredService<ILogger<Program>>();
var applicationLifetime = host.Services.GetRequiredService<IHostApplicationLifetime>();

async Task PublishNotificationAsync<TNotification>(ILogger log, IPublisher publisher, CancellationToken cancellationToken) where TNotification : IMessageNotification, new()
{
    var notificationType = typeof(TNotification).Name;

    log.LogInformation("Publishing notifications of type {Type}", notificationType);

    for (int i = 0; i < 10; i++)
    {
        log.LogInformation("Publishing notification {Number}", i);

        var startTime = Stopwatch.GetTimestamp();
    
        await publisher.Publish(new TNotification { Message = $"My message {i}" }, cancellationToken);
    
        log.LogInformation("Notification {Number} published in {Elapsed}ms", i, Stopwatch.GetElapsedTime(startTime).TotalMilliseconds);
    }
}

await PublishNotificationAsync<MyNormalNotification>(logger, mediator, terminationTokenSource.Token);
await PublishNotificationAsync<MyFireAndForgetNotification>(logger, mediator, terminationTokenSource.Token);

Console.ReadLine();

applicationLifetime.StopApplication();

await host.WaitForShutdownAsync();
