using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymMgmt.Domain.Exceptions
{
    public sealed class SubscriptionGracePeriodNegativeException : DomainException
    {
        public int AttemptedValue { get; }

        public SubscriptionGracePeriodNegativeException(int periodInDays)
            : base(
                errorCode: "SUBSCRIPTION_GRACE_PERIOD_NEGATIVE",
                message: $"Grace period cannot be negative. Received: {periodInDays} days.")
        {
            AttemptedValue = periodInDays;
        }
    }
}
