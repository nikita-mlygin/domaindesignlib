namespace DomainDesignLib.Abstractions;

public interface IEntity
{
    public object Id { get; }

    public IReadOnlyList<DomainEvent> DomainEvents { get; }
}
