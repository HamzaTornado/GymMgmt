using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymMgmt.Domain.Exceptions
{
    public sealed class MembershipPlanDurationNotPositiveException : DomainException
    {
        public int DurationInDays { get; }

        public MembershipPlanDurationNotPositiveException(int durationInDays)
            : base(
                errorCode: "MEMBERSHIP_PLAN_DURATION_NOT_POSITIVE",
                message: $"Duration must be positive. Received: {durationInDays}.")
        {
            DurationInDays = durationInDays;
        }
    }

}
