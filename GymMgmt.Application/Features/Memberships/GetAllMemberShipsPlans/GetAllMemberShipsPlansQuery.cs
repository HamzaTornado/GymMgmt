using GymMgmt.Application.Common.Interfaces;
using GymMgmt.Application.Features.Memberships.GetMemberShipPlanById;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymMgmt.Application.Features.Memberships.GetAllMemberShipsPlans
{
    public sealed record GetAllMemberShipsPlansQuery( ) : IQuery<IEnumerable<ReadMemberShipPlanDto>>;
}
