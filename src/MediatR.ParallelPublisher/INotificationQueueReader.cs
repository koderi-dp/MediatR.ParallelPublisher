namespace MediatR.ParallelPublisher;

internal interface INotificationQueueReader
{
    IAsyncEnumerable<NotificationQueueEntry> ReadAllAsync(CancellationToken cancellationToken);
}
