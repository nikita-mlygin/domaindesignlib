using System.Data;
using System.Data.Common;

namespace DomainDesignLib.Persistence.Repository;

public interface IConnectionProvider
{
    Task<DbConnection> Get();
}
