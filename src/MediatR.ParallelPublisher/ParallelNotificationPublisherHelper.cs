using System.Collections.Concurrent;

namespace MediatR.ParallelPublisher;

internal static class ParallelNotificationPublisherHelper
{
    internal static async Task<NotificationException[]> PublishAsync(NotificationHandlerExecutor[] handlers, INotification notification, CancellationToken cancellationToken)
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

        var lazyExceptions = new Lazy<ConcurrentQueue<NotificationException>>();
        
        await Parallel.ForEachAsync(handlers, new ParallelOptions { CancellationToken = cancellationToken }, async (executor, token) =>
        {
            try
            {
                await executor.HandlerCallback.Invoke(notification, token);
            }
            catch (Exception e)
            {
                lazyExceptions.Value.Enqueue(new NotificationException(executor.HandlerInstance.GetType(), e));
            }
        });

        return lazyExceptions.IsValueCreated ? lazyExceptions.Value.ToArray() : Array.Empty<NotificationException>();
    }
}
