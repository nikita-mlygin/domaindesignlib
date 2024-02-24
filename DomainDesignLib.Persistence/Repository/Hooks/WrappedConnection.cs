using System.Data;
using System.Data.Common;
using System.Diagnostics;

namespace DomainDesignLib.Persistence.Repository.Hooks;

internal class WrappedConnection : DbConnection
{
    private readonly DbConnection _connection;
    private readonly LoggingHooks _hooks;

    public WrappedConnection(DbConnection connection, LoggingHooks hooks)
    {
        _connection = connection;
        _hooks = hooks;
    }

    public override void Close()
    {
        var sw = Stopwatch.StartNew();
        try
        {
            _connection.Close();
        }
        finally
        {
            _hooks.ConnectionClosed(this, sw.ElapsedMilliseconds);
        }
    }

    public override void Open()
    {
        var sw = Stopwatch.StartNew();
        try
        {
            _connection.Open();
        }
        finally
        {
            _hooks.ConnectionOpened(this, sw.ElapsedMilliseconds);
        }
    }

    public override async Task OpenAsync(CancellationToken cancellationToken)
    {
        var sw = Stopwatch.StartNew();
        try
        {
            await _connection.OpenAsync(cancellationToken);
        }
        finally
        {
            _hooks.ConnectionOpened(this, sw.ElapsedMilliseconds);
        }
    }

    protected override DbCommand CreateDbCommand() =>
        new WrappedCommand(_connection.CreateCommand(), this, _hooks);

    //other members

    public override string ConnectionString
    {
        get => _connection.ConnectionString;
        set => _connection.ConnectionString = value;
    }

    public override int ConnectionTimeout => _connection.ConnectionTimeout;
    public override string Database => _connection.Database;
    public override string DataSource => _connection.DataSource;
    public override string ServerVersion => _connection.ServerVersion;
    public override ConnectionState State => _connection.State;

    public override void ChangeDatabase(string databaseName) =>
        _connection.ChangeDatabase(databaseName);

    protected override DbTransaction BeginDbTransaction(IsolationLevel isolationLevel) =>
        _connection.BeginTransaction(isolationLevel);

    protected override bool CanRaiseEvents => false;

    public override void EnlistTransaction(System.Transactions.Transaction transaction) =>
        _connection.EnlistTransaction(transaction);

    public override DataTable GetSchema() => _connection.GetSchema();

    public override DataTable GetSchema(string collectionName) =>
        _connection.GetSchema(collectionName);

    public override DataTable GetSchema(string collectionName, string[] restrictionValues) =>
        _connection.GetSchema(collectionName, restrictionValues);

    protected override void Dispose(bool disposing)
    {
        if (disposing)
            _connection?.Dispose();

        base.Dispose(disposing);
    }
}
