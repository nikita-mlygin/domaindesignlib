using DomainDesignLib.Abstractions;

namespace DomainDesignLib.Abstractions.Repository;

public interface IReadOnlyRepository<TEntity, TId>
    where TEntity : Entity<TId>
    where TId : notnull
{
    Task<TEntity?> GetByIdAsync(TId id);
    Task<IEnumerable<TEntity>> GetAll();
}
