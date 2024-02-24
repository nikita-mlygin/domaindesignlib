using DomainDesignLib.Abstractions;

namespace DomainDesignLib.Abstractions.Repository;

public interface IRepository<TEntity, TId> : IReadOnlyRepository<TEntity, TId>
    where TEntity : Entity<TId>
    where TId : notnull
{
    Task CreateAsync(TEntity entity);
    Task UpdateAsync(TEntity entity);
    Task RemoveAsync(TEntity entity);
}
