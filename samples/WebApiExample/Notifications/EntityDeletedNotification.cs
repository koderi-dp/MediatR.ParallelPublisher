using System.Text.Json;
using MediatR;
using MediatR.ParallelNotificationPublisher;

namespace WebApiExample.Notifications;

public class EntityDeletedNotification : IFireAndForgetNotification
{
    public object Entity { get; }
    
    public EntityDeletedNotification(object entity)
    {
        Entity = entity;
    }
}

public class EntityDeletedNotificationHandler : INotificationHandler<EntityDeletedNotification>
{
    private readonly ILogger<EntityDeletedNotificationHandler> _logger;
    
    public EntityDeletedNotificationHandler(ILogger<EntityDeletedNotificationHandler> logger)
    {
        _logger = logger;
    }
    
    public async Task Handle(EntityDeletedNotification notification, CancellationToken cancellationToken)
    {
        await Task.Delay(5000, cancellationToken);
        
        _logger.LogInformation("Entity was deleted, Data:{Payload}", JsonSerializer.Serialize(notification.Entity));
    }
}
