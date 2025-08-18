using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymMgmt.Domain.Entities.Plans
{
    public record MembershipPlanId(Guid Value)
    {
        public static MembershipPlanId New() => new(Guid.NewGuid());
        public override string ToString() => Value.ToString();
    }
}
