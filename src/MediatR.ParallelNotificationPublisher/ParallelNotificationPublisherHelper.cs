using System.Collections.Concurrent;

namespace MediatR.ParallelNotificationPublisher;

public class ParallelNotificationPublisherHelper
{
    public static async Task<NotificationException[]> PublishAsync(NotificationHandlerExecutor[] handlers, INotification notification, CancellationToken cancellationToken)
    {
        int handlersCount = handlers.Length;

        if (handlers.Length == 0)
        {
            return Array.Empty<NotificationException>();
        }
        if (handlersCount == 1)
        {
            NotificationHandlerExecutor handlerExecutor = handlers[0];
            
            try
            {
                await handlerExecutor.HandlerCallback.Invoke(notification, cancellationToken);

                return Array.Empty<NotificationException>();
            }
            catch (Exception e)
            {
                return new [] { new NotificationException(handlerExecutor.HandlerInstance.GetType(), e)};
            }
        }

        var exceptions = new ConcurrentQueue<NotificationException>();
        
        await Parallel.ForEachAsync(handlers, new ParallelOptions { CancellationToken = cancellationToken }, async (executor, token) =>
        {
            try
            {
                await executor.HandlerCallback.Invoke(notification, token);
            }
            catch (Exception e)
            {
                exceptions.Enqueue(new NotificationException(executor.HandlerInstance.GetType(), e));
            }
        });

        return exceptions.IsEmpty ? Array.Empty<NotificationException>() : exceptions.ToArray();
    }
}
