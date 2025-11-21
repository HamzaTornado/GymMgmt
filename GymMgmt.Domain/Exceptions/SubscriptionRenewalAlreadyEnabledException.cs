using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymMgmt.Domain.Exceptions
{
    public class SubscriptionRenewalAlreadyEnabledException : DomainException
    {
        public SubscriptionRenewalAlreadyEnabledException(Guid subscriptionId)
            : base(
                errorCode: "SUBSCRIPTION_ALREADY_HAS_RENEWAL_ENABLED",
                message: $"Subscription with ID {subscriptionId} already has renewal enabled.")
        {
            SubscriptionId = subscriptionId;
        }

        public Guid SubscriptionId { get; }
    }
}
