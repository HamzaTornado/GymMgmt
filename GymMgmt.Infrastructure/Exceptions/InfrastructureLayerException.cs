using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymMgmt.Infrastructure.Exceptions
{
    public class InfrastructureException : Exception
    {
        public string ErrorCode { get; }
        public InfrastructureException(string errorCode, string message)
            : base(message)
        {
            ErrorCode = errorCode ?? throw new ArgumentNullException(nameof(errorCode));
        }

        public InfrastructureException(string errorCode, string message, Exception innerException)
            : base(message, innerException)
        {
            ErrorCode = errorCode ?? throw new ArgumentNullException(nameof(errorCode));
        }
    }
}
