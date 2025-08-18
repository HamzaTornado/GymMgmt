using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymMgmt.Domain.Exceptions
{
    public class InactiveFeeCannotBeCurrentException : DomainException{

        public InactiveFeeCannotBeCurrentException()
        : base("INACTIVE_FEE", "An inactive insurance fee cannot be set as current.")
        { }
        public InactiveFeeCannotBeCurrentException(Guid feeId) : base(
            errorCode: "INACTIVE_FEE_CANNOT_BE_CURRENT",
            message: $"Cannot set inactive fee {feeId} as the current fee.")
            { }
    }
}
