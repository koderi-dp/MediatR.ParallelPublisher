namespace MediatR.ParallelNotificationPublisher;

public interface INotificationExceptionHandler
{
    ValueTask HandleAsync(INotification notification, NotificationException notificationException);
}