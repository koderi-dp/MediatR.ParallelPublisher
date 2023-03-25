namespace MediatR.ParallelNotificationPublisher;

public record NotificationException(Type NotificationHandlerType, Exception Exception);
