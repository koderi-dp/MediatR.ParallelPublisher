namespace ConsoleExample;

public class Delay
{
    public static async Task WaitAsync(int ms, CancellationToken cancellationToken)
    {
        try
        {
            await Task.Delay(ms, cancellationToken);
        }
        catch (OperationCanceledException)
        {
            // ignore
        }
    }
}