using MediatR.ParallelPublisher;
using Microsoft.Extensions.Logging;

namespace ConsoleExample.Notifications;

public class MyFireAndForgetNotification : IMessageNotification, IFireAndForgetNotification
{
    public string Message { get; init; } = string.Empty;
}

public class MyFireAndForgetNotificationHandlerOne : MessageNotificationHandlerBase<MyFireAndForgetNotification>
{
    public MyFireAndForgetNotificationHandlerOne(ILogger<MyFireAndForgetNotificationHandlerOne> logger):base(logger)
    {
    }
}

public class MyFireAndForgetNotificationHandlerTwo : MessageNotificationHandlerBase<MyFireAndForgetNotification>
{
    public MyFireAndForgetNotificationHandlerTwo(ILogger<MyFireAndForgetNotificationHandlerTwo> logger):base(logger)
    {
    }
}

public class MyFireAndForgetNotificationHandlerThree : MessageNotificationHandlerBase<MyFireAndForgetNotification>
{
    public MyFireAndForgetNotificationHandlerThree(ILogger<MyFireAndForgetNotificationHandlerThree> logger):base(logger)
    {
    }
}
