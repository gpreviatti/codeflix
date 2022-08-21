namespace Domain.Excpetions;

public class EntityValidationException : Exception
{
    public EntityValidationException(string? message) : base(message)
    {
    }
}
