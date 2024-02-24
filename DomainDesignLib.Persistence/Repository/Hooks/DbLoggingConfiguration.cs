using Microsoft.Extensions.Logging;

namespace DomainDesignLib.Persistence.Repository.Hooks;

public class DbLoggingConfiguration
{
    public static readonly DbLoggingConfiguration Default = new DbLoggingConfiguration(
        logLevel: LogLevel.Information,
        openConnectionMessage: "Dapper connection: open, elapsed: {elapsed} ms",
        closeConnectionMessage: "Dapper connection: close, elapsed: {elapsed} ms",
        executeQueryMessage: $"Dapper query:\n{{query}}\n"
            + "Parameters: {params}, elapsed: {elapsed} ms",
        logSensitiveData: false
    );

    public DbLoggingConfiguration(
        LogLevel logLevel,
        string openConnectionMessage,
        string closeConnectionMessage,
        string executeQueryMessage,
        bool logSensitiveData = false
    )
    {
        LogLevel = logLevel;
        LogSensitiveData = logSensitiveData;
        OpenConnectionMessage = openConnectionMessage ?? Default.OpenConnectionMessage;
        CloseConnectionMessage = closeConnectionMessage ?? Default.CloseConnectionMessage;
        ExecuteQueryMessage = executeQueryMessage ?? Default.ExecuteQueryMessage;
    }

    public LogLevel LogLevel { get; }
    public string OpenConnectionMessage { get; }
    public string CloseConnectionMessage { get; }
    public string ExecuteQueryMessage { get; }
    public bool LogSensitiveData { get; }
}
