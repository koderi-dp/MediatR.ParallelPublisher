using System.Text.Json;
using MediatR;
using MediatR.ParallelPublisher;

namespace WebApiExample.Notifications;

public class EntityAddedNotification : IFireAndForgetNotification
{
    public object Entity { get; }
    
    public EntityAddedNotification(object entity)
    {
        Entity = entity;
    }
}

public class EntityAddedNotificationHandler : INotificationHandler<EntityAddedNotification>
{
    private readonly ILogger<EntityAddedNotificationHandler> _logger;
    
    public EntityAddedNotificationHandler(ILogger<EntityAddedNotificationHandler> logger)
    {
        _logger = logger;
    }
    
    public async Task Handle(EntityAddedNotification notification, CancellationToken cancellationToken)
    {
        await Task.Delay(5000, cancellationToken);
        
        _logger.LogInformation("Entity was added, Data:{Payload}", JsonSerializer.Serialize(notification.Entity));
    }
}

public class EntityAddedThrowsExceptionNotificationHandler : INotificationHandler<EntityAddedNotification>
{
    public async Task Handle(EntityAddedNotification notification, CancellationToken cancellationToken)
    {
        await Task.Delay(1000, cancellationToken);

        throw new InvalidOperationException($"Failed to process entity {JsonSerializer.Serialize(notification.Entity)}");
    }
}
