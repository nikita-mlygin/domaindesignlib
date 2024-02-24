using DomainDesignLib.Abstractions;
using DomainDesignLib.Abstractions.Repository;
using DomainDesignLib.Persistence.EntityTracking;

namespace DomainDesignLib.Persistence.Repository.EventTracking;

public class RepositoryEventTrackingDecorator<TEntity, TId>(
    IEntityTracking entityTracking,
    IRepository<TEntity, TId> repository
) : IRepository<TEntity, TId>
    where TEntity : Entity<TId>
    where TId : notnull
{
    private readonly IRepository<TEntity, TId> repository = repository;
    private readonly IEntityTracking entityTracking = entityTracking;

    public async Task CreateAsync(TEntity entity)
    {
        await repository.CreateAsync(entity);
        entityTracking.AddToTracking(entity);
    }

    public async Task<IEnumerable<TEntity>> GetAll()
    {
        var entities = await repository.GetAll();
        entityTracking.AddToTracking(entities);
        return entities;
    }

    public Task<IEnumerable<TEntity>> GetAllWithoutTracking() => repository.GetAll();

    public async Task<TEntity> GetByIdAsync(TId id)
    {
        var entity = await repository.GetByIdAsync(id);
        entityTracking.AddToTracking(entity);
        return entity;
    }

    public Task<TEntity> GetByIdAsyncWithoutTracking(TId id) => repository.GetByIdAsync(id);

    public Task RemoveAsync(TEntity entity) => repository.RemoveAsync(entity);

    public Task UpdateAsync(TEntity entity) => repository.UpdateAsync(entity);
}
