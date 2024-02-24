namespace DomainDesignLib.Persistence.Repository.Hooks;

using System.Data.Common;
using Dapper.Logging;
using Microsoft.Extensions.Logging;

public class LoggingHooks
{
    private readonly ILogger logger;
    private readonly DbLoggingConfiguration config;

    public LoggingHooks(ILogger logger, DbLoggingConfiguration config)
    {
        this.logger = logger;
        this.config = config;
    }

    public void ConnectionOpened(DbConnection connection, long elapsedMs) =>
        logger.Log(config.LogLevel, config.OpenConnectionMessage, elapsedMs);

    public void ConnectionClosed(DbConnection connection, long elapsedMs) =>
        logger.Log(config.LogLevel, config.CloseConnectionMessage, elapsedMs);

    public void CommandExecuted(DbCommand command, long elapsedMs) =>
        logger.Log(
            config.LogLevel,
            config.ExecuteQueryMessage,
            command.CommandText,
            command.GetParameters(hideValues: !config.LogSensitiveData),
            elapsedMs
        );
}
