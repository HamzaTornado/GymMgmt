using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymMgmt.Domain.Exceptions
{
    public class NoActiveSubscriptionException : DomainException
    {
        public NoActiveSubscriptionException(Guid memberId, string action) : base(
                errorCode: "NO_ACTIVE_SUBSCRIPTION",
                message: $"Member {memberId} has no active subscription to {action}.")
        { }
    }
}
