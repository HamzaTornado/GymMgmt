using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymMgmt.Domain.Exceptions
{
    public sealed class SubscriptionAlreadyActiveException : DomainException
    {
        public SubscriptionAlreadyActiveException(Guid subscriptionId)
            : base(
                errorCode: "SUBSCRIPTION_ALREADY_ACTIVE",
                message: $"Subscription {subscriptionId} is already active.")
        {
            SubscriptionId = subscriptionId;
        }

        public Guid SubscriptionId { get; }
    }
}
