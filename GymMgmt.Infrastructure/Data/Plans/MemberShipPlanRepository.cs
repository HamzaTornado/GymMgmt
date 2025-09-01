using GymMgmt.Domain.Common;
using GymMgmt.Domain.Entities.Plans;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymMgmt.Infrastructure.Data.Plans
{
    public class MemberShipPlanRepository(GymDbContext dbContext) : BaseRepository<MembershipPlan,MembershipPlanId>(dbContext), IAggregateRoot
    {
    }
}
