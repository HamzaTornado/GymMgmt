using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymMgmt.Infrastructure.Exceptions
{
    /// <summary>
    /// Thrown when a required DbContext instance is null.
    /// </summary>
    public class DbContextNullException : InfrastructureException
    {
        public DbContextNullException()
            : base(
                errorCode: "DB_CONTEXT_NULL",
                message: "The provided DbContext instance cannot be null.")
        {
        }

        public DbContextNullException(string contextName)
            : base(
                errorCode: "DB_CONTEXT_NULL",
                message: $"The required DbContext '{contextName}' instance cannot be null.")
        {
        }

        public DbContextNullException(string? message, Exception? innerException = null)
            : base(
                errorCode: "DB_CONTEXT_NULL",
                message: message ?? "The provided DbContext instance cannot be null.",
                innerException: innerException)
        {
        }
    }
}
