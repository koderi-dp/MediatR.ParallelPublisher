using System.Collections.Concurrent;
using System.Reflection;

namespace MediatR.ParallelNotificationPublisher;

public class ParallelNotificationPublisher : INotificationPublisher
{
    private readonly INotificationQueueWriter _queueWriter;
    private readonly IEnumerable<INotificationExceptionHandler> _exceptionHandlers;
    private static readonly ConcurrentDictionary<Type, FireAndForgetNotificationAttribute?> FireAndForgetNotificationTypes = new ConcurrentDictionary<Type, FireAndForgetNotificationAttribute?>();
    
    public ParallelNotificationPublisher(INotificationQueueWriter queueWriter, IEnumerable<INotificationExceptionHandler> exceptionHandlers)
    {
        _queueWriter = queueWriter;
        _exceptionHandlers = exceptionHandlers;
    }
    
    public async Task Publish(IEnumerable<NotificationHandlerExecutor> handlerExecutors, INotification notification, CancellationToken cancellationToken)
    {
        if (IsFireAndForgetNotification(notification))
        {
            await _queueWriter.WriteAsync(handlerExecutors.ToArray(), notification, cancellationToken);
            return;
        }

        var notificationExceptions = await ParallelNotificationPublisherHelper.PublishAsync(handlerExecutors.ToArray(), notification, cancellationToken);

        if(notificationExceptions.Length > 0)
            await ProcessExceptionsAsync(notificationExceptions, notification);
    }

    private async ValueTask ProcessExceptionsAsync(IEnumerable<NotificationException> notificationExceptions, INotification notification)
    {
        foreach (var notificationException in notificationExceptions)
        {
            foreach (var exceptionHandler in _exceptionHandlers)
            {
                await exceptionHandler.HandleAsync(notification, notificationException);
            }
        }
    }
    
    private static bool IsFireAndForgetNotification(INotification notification)
    {
        return notification is IFireAndForgetNotification || FireAndForgetNotificationTypes.GetOrAdd(notification.GetType(), type => type.GetCustomAttribute<FireAndForgetNotificationAttribute>()) != null;
    }
}
