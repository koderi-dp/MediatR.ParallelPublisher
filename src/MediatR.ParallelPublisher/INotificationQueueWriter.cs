namespace MediatR.ParallelPublisher;

internal interface INotificationQueueWriter
{
    ValueTask WriteAsync(NotificationHandlerExecutor[] handlers, INotification notification, CancellationToken cancellationToken);
}
