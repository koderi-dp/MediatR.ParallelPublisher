namespace WebApiExample.Exceptions;

public class EntityNotFoundException : Exception
{
    public static readonly EntityNotFoundException Instance = new EntityNotFoundException();
}
