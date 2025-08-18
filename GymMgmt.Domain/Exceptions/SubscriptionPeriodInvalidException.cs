using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymMgmt.Domain.Exceptions
{
    public sealed class SubscriptionPeriodInvalidException : DomainException
    {
        public SubscriptionPeriodInvalidException(DateTime start, DateTime end)
            : base(
                errorCode: "SUBSCRIPTION_PERIOD_INVALID",
                message: $"Period start ({start:yyyy-MM-dd}) must be before end ({end:yyyy-MM-dd}).")
        {
            PeriodStart = start;
            PeriodEnd = end;
        }

        public DateTime PeriodStart { get; }
        public DateTime PeriodEnd { get; }
    }
}
