namespace Domain.Excpetions;

public class EntityValidationExcpetion : Exception
{
    public EntityValidationExcpetion(string? message) : base(message)
    {
    }
}
