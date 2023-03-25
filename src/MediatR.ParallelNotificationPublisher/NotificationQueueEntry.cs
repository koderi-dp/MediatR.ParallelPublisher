namespace MediatR.ParallelNotificationPublisher;

public record NotificationQueueEntry(NotificationHandlerExecutor[] Handlers, INotification Notification);