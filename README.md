# MediatR.ParallelPublisher

MediatR.ParallelPublisher is an extension for [MediatR](https://github.com/jbogard/MediatR) that enables parallel and fire-and-forget publishing of notifications. The library aims to improve performance and responsiveness by executing notification handlers concurrently, while also providing exception handling mechanisms for notification processing.

## Features

- Parallel execution of notification handlers
- Fire-and-forget notifications
- Customizable exception handling

## Installation

Install MediatR.ParallelPublisher via NuGet Package Manager or by running the following command:

```sh
dotnet add package MediatR.ParallelPublisher
```

## Usage

After installing the package, register the parallel publisher in your dependency injection container:

```csharp
services.AddMediatR(config =>
{
    config.UseParallelNotificationPublisher(options =>
    {
        // Register custom exception handlers (optional)
        options.RegisterExceptionHandler<MyCustomExceptionHandler>();
    });
});
```

To make a notification fire-and-forget, either implement the `IFireAndForgetNotification` interface or use the 
`FireAndForgetNotificationAttribute`:

```csharp
// Using the IFireAndForgetNotification interface
public class MyNotification : INotification, IFireAndForgetNotification
{
    // ...
}

// Using the FireAndForgetNotificationAttribute
[FireAndForgetNotification]
public class MyOtherNotification : INotification
{
    // ...
}
```

Implement your notification handlers as usual, and they will be executed in parallel or fire-and-forget mode, depending on the notification type:

```csharp
public class MyNotificationHandler : INotificationHandler<MyNotification>
{
    public Task Handle(MyNotification notification, CancellationToken cancellationToken)
    {
        // Your handler implementation
    }
}
```

For custom exception handling, implement the `INotificationExceptionHandler` interface:
```csharp
public class MyCustomExceptionHandler : INotificationExceptionHandler
{
    public Task HandleAsync(INotification notification, NotificationException exception)
    {
        // Handle the exception for the specified notification
    }
}
```

## License

MediatR.ParallelPublisher is released under the Apache License. See the LICENSE file for details.

## Contributing
Contributions are welcome! Feel free to submit issues, feature requests, or pull requests.