using Microsoft.Extensions.DependencyInjection;

namespace MediatR.ParallelNotificationPublisher;

public static class DependencyRegistration
{
    public static void AddParallelNotificationPublisher(this MediatRServiceConfiguration configuration, IServiceCollection services, Action<IParallelNotificationPublisherOptions>? configure = null)
    {
        var options = new ParallelNotificationPublisherOptions();
        configure?.Invoke(options);
        
        services.AddSingleton<NotificationQueue>();
        services.AddSingleton<INotificationQueueWriter>(provider => provider.GetRequiredService<NotificationQueue>());
        services.AddSingleton<INotificationQueueReader>(provider => provider.GetRequiredService<NotificationQueue>());
        services.AddHostedService<NotificationQueueService>();
        configuration.NotificationPublisherType = typeof(ParallelNotificationPublisher);

        if (options.RegisteredExceptionHandlerTypes.Count == 0)
        {
            options.RegisterExceptionHandler<DefaultNotificationExceptionHandler>();
        }
        
        foreach (Type handlerType in options.RegisteredExceptionHandlerTypes)
        {
            services.Add(ServiceDescriptor.Describe(typeof(INotificationExceptionHandler), handlerType, configuration.Lifetime));
        }
    }
}
