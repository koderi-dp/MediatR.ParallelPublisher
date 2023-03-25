using System.Threading.Channels;

namespace MediatR.ParallelNotificationPublisher;

internal class NotificationQueue : INotificationQueueWriter, INotificationQueueReader
{
    private readonly Channel<NotificationQueueEntry> _queue = Channel.CreateUnbounded<NotificationQueueEntry>(new UnboundedChannelOptions { SingleReader = true, SingleWriter = false });

    public async ValueTask WriteAsync(NotificationHandlerExecutor[] handlers, INotification notification, CancellationToken cancellationToken)
    {
        await _queue.Writer.WriteAsync(new NotificationQueueEntry(handlers, notification), cancellationToken);
    }

    public IAsyncEnumerable<NotificationQueueEntry> ReadAllAsync(CancellationToken cancellationToken) => _queue.Reader.ReadAllAsync(cancellationToken);
}
