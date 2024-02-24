using DomainDesignLib.Abstractions;
using DomainDesignLib.Abstractions.Repository;

namespace DomainDesignLib.Persistence.Repository;

public abstract class DatabaseRepository<TEntity, TId>(IConnectionProvider connectionProvider)
    : IRepository<TEntity, TId>
    where TEntity : Entity<TId>
    where TId : notnull
{
    protected readonly IConnectionProvider connectionProvider = connectionProvider;

    public abstract Task CreateAsync(TEntity entity);
    public abstract Task<IEnumerable<TEntity>> GetAll();
    public abstract Task<TEntity?> GetByIdAsync(TId id);
    public abstract Task RemoveAsync(TEntity entity);
    public abstract Task UpdateAsync(TEntity entity);
}
