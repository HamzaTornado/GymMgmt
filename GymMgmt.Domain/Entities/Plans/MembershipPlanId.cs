using GymMgmt.Domain.Entities.Members;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymMgmt.Domain.Entities.Plans
{
    public record MembershipPlanId(Guid Value)
    {
        public static MembershipPlanId FromValue(Guid value) => new(value);
        public static MembershipPlanId New() => new(Guid.NewGuid());
    }
}
