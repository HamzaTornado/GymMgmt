using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymMgmt.Domain.Exceptions
{
    public sealed class MembershipPlanNameRequiredException : DomainException
    {
        public MembershipPlanNameRequiredException()
            : base(
                errorCode: "MEMBERSHIP_PLAN_NAME_REQUIRED",
                message: "Membership-plan name is required.")
        { }
    }
}
