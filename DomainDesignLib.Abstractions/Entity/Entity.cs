namespace DomainDesignLib.Abstractions;

/// <summary>
/// Base class for all entities.
/// </summary>
/// <typeparam name="T">The type of the entity identifier.</typeparam>
public abstract class Entity<T>(T id) : IEntity
    where T : notnull
{
    private readonly List<DomainEvent> domainEvents = [];

    protected void Raise(DomainEvent domainEvent) => domainEvents.Add(domainEvent);

    public IReadOnlyList<DomainEvent> DomainEvents => [..domainEvents];

    /// <summary>
    /// Gets the entity identifier.
    /// </summary>
    public T Id { get; } = id;

    object IEntity.Id => Id;

    /// <summary>
    /// Indicates whether the current object is equal to another object of the same type.
    /// </summary>
    /// <param name="obj">The object to compare with the current object.</param>
    /// <returns>
    /// true if the current object is equal to the obj parameter; otherwise, false.
    /// </returns>
    public override bool Equals(object? obj)
    {
        if (obj is null)
        {
            return false;
        }

        if (obj is Entity<T> entity)
        {
            return entity.GetHashCode() == this.GetHashCode();
        }

        return false;
    }

    /// <summary>
    /// Returns the hash code for this instance by entity id.
    /// </summary>
    /// <returns>
    /// A 32-bit signed integer that is the hash code for this instance.
    /// </returns>
    public override int GetHashCode()
    {
        return this.Id.GetHashCode();
    }
}
