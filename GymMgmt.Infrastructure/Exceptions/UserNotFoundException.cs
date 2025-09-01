using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymMgmt.Infrastructure.Exceptions
{
    /// <summary>
    /// Thrown when a user cannot be found by username, email, or ID.
    /// </summary>
    public class UserNotFoundException : InfrastructureException
    {
        public string? UserId { get; }
        public string? Username { get; }

        public UserNotFoundException()
            : base(
                errorCode: "USER_NOT_FOUND",
                message: "The requested user could not be found.")
        {
        }

        public UserNotFoundException(Guid userId)
            : base(
                errorCode: "USER_NOT_FOUND",
                message: $"User with ID '{userId}' could not be found.")
        {
            UserId = userId.ToString();
        }
        public UserNotFoundException(string userName)
            : base(
                errorCode: "USER_NOT_FOUND",
                message: $"User with username '{userName}' could not be found.")
        {
            UserId = userName;
        }
        public UserNotFoundException(string message, Exception? innerException = null)
            : base(
                errorCode: "USER_NOT_FOUND",
                message: message,
                innerException: innerException)
        {
        }
    }
}
