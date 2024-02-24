namespace DomainDesignLib.Abstractions;

public abstract class IntegrationEvent<T>(Guid id, T message)
{
    public Guid Id { get; } = id;
    public T Message { get; } = message;
}
