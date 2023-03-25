namespace MediatR.ParallelNotificationPublisher;

public interface INotificationQueueReader
{
    IAsyncEnumerable<NotificationQueueEntry> ReadAllAsync(CancellationToken cancellationToken);
}
