using Microsoft.Extensions.Logging;

namespace ConsoleExample.Notifications;

public class MyNormalNotification : IMessageNotification
{
    public string Message { get; init; } = string.Empty;
}

public class MyNormalNotificationHandlerOne : MessageNotificationHandlerBase<MyNormalNotification>
{
    public MyNormalNotificationHandlerOne(ILogger<MyNormalNotificationHandlerOne> logger):base(logger)
    {
    }
}
public class MyNormalNotificationHandlerTwo : MessageNotificationHandlerBase<MyNormalNotification>
{
    public MyNormalNotificationHandlerTwo(ILogger<MyNormalNotificationHandlerTwo> logger):base(logger)
    {
    }
}

public class MyNormalNotificationHandlerThree : MessageNotificationHandlerBase<MyNormalNotification>
{
    public MyNormalNotificationHandlerThree(ILogger<MyNormalNotificationHandlerThree> logger):base(logger)
    {
    }
}

