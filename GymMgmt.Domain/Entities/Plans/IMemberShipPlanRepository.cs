using GymMgmt.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymMgmt.Domain.Entities.Plans
{
    public interface IMemberShipPlanRepository : IBaseRepository<MembershipPlan, MembershipPlanId>
    {
    }
}
