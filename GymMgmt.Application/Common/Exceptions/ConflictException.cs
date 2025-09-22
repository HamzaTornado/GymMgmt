using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymMgmt.Application.Common.Exceptions
{
    public class ConflictException : ApplicationLayerException
    {
        public ConflictException()
            : base("CONFLICT", "The request could not be completed due to a conflict with the current state of the resource.")
        {
        }

        public ConflictException(string message)
            : base("CONFLICT", message)
        {
        }

        public ConflictException(string message, Exception innerException)
            : base("CONFLICT", message, innerException)
        {
        }
    }
}
