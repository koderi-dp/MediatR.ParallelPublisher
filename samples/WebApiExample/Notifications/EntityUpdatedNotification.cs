using MediatR.ParallelPublisher;

namespace WebApiExample.Notifications;

public class EntityUpdatedNotification : IFireAndForgetNotification
{
    public object Entity { get; }
    
    public EntityUpdatedNotification(object entity)
    {
        Entity = entity;
    }
}
