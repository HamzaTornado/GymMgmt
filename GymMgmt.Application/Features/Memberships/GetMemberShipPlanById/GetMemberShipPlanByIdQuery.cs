using GymMgmt.Application.Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymMgmt.Application.Features.Memberships.GetMemberShipPlanById
{
    public sealed record GetMemberShipPlanByIdQuery(Guid MemberShipPlanId) : IQuery<ReadMemberShipPlanDto>;
}
