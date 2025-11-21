using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymMgmt.Domain.Exceptions
{
    public class SubscriptionExpiredException : DomainException
    {
        public SubscriptionExpiredException(Guid subscriptionId)
            : base(
                errorCode: "SUBSCRIPTION_IS_EXPIRED",
                message: $"Subscription with Id: {subscriptionId} is already expired.")
        {
            SubscriptionId = subscriptionId;
        }

        public Guid SubscriptionId { get; }
    }
}
