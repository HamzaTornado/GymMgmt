using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymMgmt.Domain.Exceptions
{
    /// <summary>
    /// Base exception type for domain-related failures.
    /// All business rule violations should throw a derived type.
    /// </summary>
    public abstract class DomainException : Exception
    {
        /// <summary>
        /// A short code that identifies the error type (useful for clients).
        /// </summary>
        public string ErrorCode { get; }

        /// <summary>
        /// Optional user-friendly message (can be localized later).
        /// </summary>
        public string? UserMessage { get; }

        protected DomainException(string errorCode, string? message = null, Exception? innerException = null)
            : base(message, innerException)
        {
            ErrorCode = errorCode ?? throw new ArgumentNullException(nameof(errorCode));
            UserMessage = message;
        }
    }
}
