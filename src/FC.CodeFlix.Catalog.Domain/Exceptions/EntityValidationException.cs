namespace FC.CodeFlix.Catalog.Domain.Exceptions;

public class EntityValidationException: BusinessRuleException
{
    public EntityValidationException(string? message):base(message) {
    }
}
