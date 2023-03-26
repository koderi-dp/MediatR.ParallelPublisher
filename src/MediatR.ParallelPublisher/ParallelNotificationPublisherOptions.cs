namespace MediatR.ParallelPublisher;

public interface IParallelNotificationPublisherOptions
{
    void RegisterExceptionHandler<THandler>() where THandler : INotificationExceptionHandler;
}

internal sealed class ParallelNotificationPublisherOptions : IParallelNotificationPublisherOptions
{
    private readonly List<Type> _exceptionHandlerTypes = new List<Type>();
    
    public void RegisterExceptionHandler<THandler>() where THandler : INotificationExceptionHandler
    {
        var handlerType = typeof(THandler);
        
        if (!_exceptionHandlerTypes.Contains(handlerType))
        {
            _exceptionHandlerTypes.Add(handlerType);
        }
    }

    public IReadOnlyList<Type> RegisteredExceptionHandlerTypes => _exceptionHandlerTypes;
}
