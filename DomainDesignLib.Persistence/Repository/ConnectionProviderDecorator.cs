using System.Data;
using System.Data.Common;
using DomainDesignLib.Persistence.Repository.Hooks;
using Microsoft.Extensions.Logging;

namespace DomainDesignLib.Persistence.Repository;

public class ConnectionProviderDecorator(
    IConnectionProvider connectionProvider,
    ILogger logger,
    DbLoggingConfiguration configuration
) : IConnectionProvider
{
    private readonly IConnectionProvider connectionProvider = connectionProvider;
    private readonly ILogger logger = logger;
    private readonly DbLoggingConfiguration configuration = configuration;

    public async Task<DbConnection> Get()
    {
        return new WrappedConnection(
            await connectionProvider.Get(),
            new LoggingHooks(logger, configuration)
        );
    }
}
