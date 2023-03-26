namespace MediatR.ParallelPublisher;

internal sealed record NotificationQueueEntry(NotificationHandlerExecutor[] Handlers, INotification Notification);