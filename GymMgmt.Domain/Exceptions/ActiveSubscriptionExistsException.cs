using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymMgmt.Domain.Exceptions
{
    public class ActiveSubscriptionExistsException : DomainException
    {
        public ActiveSubscriptionExistsException(Guid memberId) : base(
            errorCode: "ACTIVE_SUBSCRIPTION_EXISTS",
            message: $"Cannot start a new subscription while member {memberId} has an active one.")
        { }
    }
}
