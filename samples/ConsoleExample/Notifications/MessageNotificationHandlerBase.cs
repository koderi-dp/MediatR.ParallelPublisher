using MediatR;
using Microsoft.Extensions.Logging;

namespace ConsoleExample.Notifications;

public abstract class MessageNotificationHandlerBase<TMessage> : INotificationHandler<TMessage> where TMessage : IMessageNotification
{
    private readonly ILogger _logger;
    
    protected MessageNotificationHandlerBase(ILogger logger)
    {
        _logger = logger;
    }
    
    public async Task Handle(TMessage notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Received notification <{Message}>", notification.Message);

        await Delay.WaitAsync(1000, cancellationToken);
    }
}
