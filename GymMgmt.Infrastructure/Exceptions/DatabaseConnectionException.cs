using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymMgmt.Infrastructure.Exceptions
{
    /// <summary>
    /// Thrown when database connection fails.
    /// </summary>
    public class DatabaseConnectionException : InfrastructureException
    {
        public string? ConnectionString { get; }

        public DatabaseConnectionException()
            : base(
                errorCode: "DATABASE_CONNECTION_FAILED",
                message: "Failed to establish database connection. Please check connection string and database availability." )
        {
            
        }

        public DatabaseConnectionException(string message, string connectionString, Exception? innerException = null)
            : base(
                errorCode: "DATABASE_CONNECTION_FAILED",
                message: message,
                innerException: innerException)
        {
            ConnectionString = connectionString;
        }
    }
}
