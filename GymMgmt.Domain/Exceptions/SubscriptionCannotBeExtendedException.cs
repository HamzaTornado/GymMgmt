using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymMgmt.Domain.Exceptions
{
    public class SubscriptionCannotBeExtendedException : DomainException
    {
        public SubscriptionCannotBeExtendedException(Guid subscriptionId)
            : base(
                errorCode: "SUBSCRIPTION_CANNOT_BE_EXTENDED",
                message: $"Subscription with Id: {subscriptionId} can not be extended.")
        {
            SubscriptionId = subscriptionId;
        }

        public Guid SubscriptionId { get; }
    }
}
