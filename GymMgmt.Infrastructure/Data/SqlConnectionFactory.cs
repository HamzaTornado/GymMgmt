using GymMgmt.Application.Common.Interfaces;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;
using System.Data;


namespace GymMgmt.Infrastructure.Data
{
    public class SqlConnectionFactory : ISqlConnectionFactory, IDisposable
    {
        private readonly string _connectionString;
        private readonly ILogger<SqlConnectionFactory> _logger;
        private IDbConnection? _connection;
        private bool _disposed = false;

        public SqlConnectionFactory(string connectionString, ILogger<SqlConnectionFactory> logger)
        {
            _connectionString = connectionString;
            _logger = logger;
        }

        public IDbConnection GetOpenConnection()
        {
            if (_disposed)
                throw new ObjectDisposedException(nameof(SqlConnectionFactory));

            if (_connection is null || _connection.State != ConnectionState.Open)
            {
                _connection?.Dispose();
                _connection = new SqlConnection(_connectionString);
                _connection.Open();
                _logger.LogDebug("Created and opened new SQL Server connection");
            }

            return _connection;
        }

        public IDbConnection CreateNewConnection()
        {
            if (_disposed)
                throw new ObjectDisposedException(nameof(SqlConnectionFactory));

            var connection = new SqlConnection(_connectionString);
            connection.Open();
            _logger.LogDebug("Created new dedicated SQL Server connection");
            return connection;
        }

        public string GetConnectionString() => _connectionString;

        public void Dispose()
        {
            if (!_disposed)
            {
                _connection?.Dispose();
                _connection = null;
                _disposed = true;
                _logger.LogDebug("Disposed SQL Server connection factory");
            }
            GC.SuppressFinalize(this);
        }
    }
}
