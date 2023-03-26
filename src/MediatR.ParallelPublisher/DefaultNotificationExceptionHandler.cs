using Microsoft.Extensions.Logging;

namespace MediatR.ParallelPublisher;

internal sealed class DefaultNotificationExceptionHandler : INotificationExceptionHandler
{
    private readonly ILogger<DefaultNotificationExceptionHandler> _logger;
    
    public DefaultNotificationExceptionHandler(ILogger<DefaultNotificationExceptionHandler> logger)
    {
        _logger = logger;
    }
    
    public ValueTask HandleAsync(INotification notification, NotificationException notificationException)
    {
        _logger.LogError(notificationException.Exception, "Unhandled exception occured in handler {HandlerType} while processing notification type {NotificationType}",
            notificationException.NotificationHandlerType.Name, notification.GetType().Name);
        
        return ValueTask.CompletedTask;
    }
}
