using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymMgmt.Infrastructure.Exceptions
{
    public abstract class InfrastructureException : Exception
    {
        /// <summary>
        /// A stable, machine-readable code (e.g., "DATABASE_CONNECTION_FAILED").
        /// </summary>
        public string ErrorCode { get; }

        protected InfrastructureException(string errorCode, string message)
            : base(message)
        {
            ErrorCode = errorCode ?? throw new ArgumentNullException(nameof(errorCode));
        }

        protected InfrastructureException(string errorCode, string message, Exception innerException)
            : base(message, innerException)
        {
            ErrorCode = errorCode ?? throw new ArgumentNullException(nameof(errorCode));
        }
    }
}
