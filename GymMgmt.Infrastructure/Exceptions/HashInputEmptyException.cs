using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymMgmt.Infrastructure.Exceptions
{
    /// <summary>
    /// Thrown when hash input is null, empty, or whitespace.
    /// </summary>
    public class HashInputEmptyException : InfrastructureException
    {
        public HashInputEmptyException()
            : base(
                errorCode: "HASH_INPUT_EMPTY",
                message: "Hash input cannot be null, empty, or whitespace.")
        {
        }

        public HashInputEmptyException(string? inputName)
            : base(
                errorCode: "HASH_INPUT_EMPTY",
                message: $"Hash input '{inputName}' cannot be null, empty, or whitespace.")
        {
        }
    }
}
