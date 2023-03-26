using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace MediatR.ParallelPublisher;

internal sealed class NotificationQueueService : BackgroundService
{
    private readonly ILogger<NotificationQueueService> _logger;
    private readonly INotificationQueueReader _queueReader;
    private readonly IEnumerable<INotificationExceptionHandler> _exceptionHandlers;

    public NotificationQueueService(ILogger<NotificationQueueService> logger, INotificationQueueReader queueReader, IEnumerable<INotificationExceptionHandler> exceptionHandlers)
    {
        _logger = logger;
        _queueReader = queueReader;
        _exceptionHandlers = exceptionHandlers;
    }
    
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        try
        {
            await foreach (NotificationQueueEntry entry in _queueReader.ReadAllAsync(stoppingToken))
            {
                var notificationExceptions = await ParallelNotificationPublisherHelper.PublishAsync(entry.Handlers, entry.Notification, stoppingToken);

                if(notificationExceptions.Length > 0)
                    await ProcessExceptionsAsync(notificationExceptions, entry.Notification);
            }
        }
        catch (OperationCanceledException)
        {
            // Ignore
        }
    }

    private async ValueTask ProcessExceptionsAsync(IEnumerable<NotificationException> notificationExceptions, INotification notification)
    {
        foreach (NotificationException notificationException in notificationExceptions)
        {
            foreach (INotificationExceptionHandler exceptionHandler in _exceptionHandlers)
            {
                try
                {
                    await exceptionHandler.HandleAsync(notification, notificationException);
                }
                catch (Exception e)
                {
                    _logger.LogError(e, "Failed to handle exception for notification type {NotificationType}", notification.GetType().Name);
                }
            }
        }
    }
}
