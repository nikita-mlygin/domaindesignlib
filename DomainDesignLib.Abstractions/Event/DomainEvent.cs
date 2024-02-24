namespace DomainDesignLib.Abstractions;

using MediatR;

public abstract class DomainEvent(Guid id) : INotification
{
    public Guid Id { get; } = id;
}
