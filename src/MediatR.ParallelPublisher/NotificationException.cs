namespace MediatR.ParallelPublisher;

public sealed record NotificationException(Type NotificationHandlerType, Exception Exception);
