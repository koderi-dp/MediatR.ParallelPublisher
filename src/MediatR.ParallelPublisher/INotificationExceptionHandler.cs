namespace MediatR.ParallelPublisher;

public interface INotificationExceptionHandler
{
    ValueTask HandleAsync(INotification notification, NotificationException notificationException);
}