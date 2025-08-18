using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymMgmt.Domain.Exceptions
{
    public class MemberNameRequiredException : DomainException
    {
        public MemberNameRequiredException(string missingField)
        : base(
            errorCode: "MEMBER_NAME_REQUIRED",
            message: $"The {missingField} is required and cannot be empty.")
        {
        }
    }
}
