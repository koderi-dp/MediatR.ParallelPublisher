namespace MediatR.ParallelNotificationPublisher;

public interface INotificationQueueWriter
{
    ValueTask WriteAsync(NotificationHandlerExecutor[] handlers, INotification notification, CancellationToken cancellationToken);
}
