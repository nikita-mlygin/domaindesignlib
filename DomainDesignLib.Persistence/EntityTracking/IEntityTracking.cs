using DomainDesignLib.Abstractions;

namespace DomainDesignLib.Persistence.EntityTracking;

public interface IEntityTracking
{
    public void AddToTracking<T>(Entity<T> entity)
        where T : notnull;

    public void AddToTracking<T>(IEnumerable<Entity<T>> entities)
        where T : notnull;

    public IReadOnlyList<IEntity> TrackingEntities { get; }
}
