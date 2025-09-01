using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymMgmt.Infrastructure.Exceptions
{
    /// <summary>
    /// Thrown when a JWT token is malformed, invalid, or cannot be validated.
    /// </summary>
    public class InvalidTokenException : InfrastructureException
    {
        public InvalidTokenException()
            : base(
                errorCode: "TOKEN_INVALID",
                message: "The provided token is invalid or expired.")
        {
        }

        public InvalidTokenException(string tokenType)
            : base(
                errorCode: "TOKEN_INVALID",
                message: $"The {tokenType} token is invalid or expired.")
        {
        }

        public InvalidTokenException(string message, Exception? innerException = null)
            : base(
                errorCode: "TOKEN_INVALID",
                message: message,
                innerException: innerException)
        {
        }
    }
}
