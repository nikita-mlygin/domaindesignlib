using DomainDesignLib.Abstractions;

namespace DomainDesignLib.Persistence.EntityTracking;

public class EntityTracking : IEntityTracking
{
    private readonly List<IEntity> entities = [];

    public IReadOnlyList<IEntity> TrackingEntities => [..entities];

    public void AddToTracking<T>(Entity<T> entity)
        where T : notnull
    {
        entities.Add(entity);
    }

    public void AddToTracking<T>(IEnumerable<Entity<T>> entities)
        where T : notnull
    {
        entities.ToList().ForEach(x => this.entities.Add(x));
    }
}
