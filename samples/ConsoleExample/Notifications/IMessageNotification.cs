using MediatR;

namespace ConsoleExample.Notifications;

public interface IMessageNotification : INotification
{
    string Message { get; init; }
}
