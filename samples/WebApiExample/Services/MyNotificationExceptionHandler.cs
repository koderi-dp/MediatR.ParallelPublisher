using System.Text.Json;
using MediatR;
using MediatR.ParallelNotificationPublisher;

namespace WebApiExample.Services;

public class MyNotificationExceptionHandler : INotificationExceptionHandler
{
    private readonly ILogger<MyNotificationExceptionHandler> _logger;
    
    public MyNotificationExceptionHandler(ILogger<MyNotificationExceptionHandler> logger)
    {
        _logger = logger;
    }
    
    public ValueTask HandleAsync(INotification notification, NotificationException notificationException)
    {
        // handle exception
        _logger.LogError(notificationException.Exception, "Received unhandled exception from handler {Handler} and notification {Notification}",
           notificationException.NotificationHandlerType.Name, notification.GetType().Name);
        return ValueTask.CompletedTask;
    }
}
