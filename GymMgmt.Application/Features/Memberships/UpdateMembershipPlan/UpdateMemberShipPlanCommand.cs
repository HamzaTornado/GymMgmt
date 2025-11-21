using GymMgmt.Application.Common.Interfaces;
using GymMgmt.Application.Features.Memberships.GetMemberShipPlanById;
using GymMgmt.Domain.Entities.Plans;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymMgmt.Application.Features.Memberships.UpdateMembershipPlan
{
    public sealed record UpdateMemberShipPlanCommand(
        Guid Id,
        string Name,
        int DurationInMonths,
        decimal Price,
        bool IsActive
        ) :ICommand<ReadMemberShipPlanDto>;
}
