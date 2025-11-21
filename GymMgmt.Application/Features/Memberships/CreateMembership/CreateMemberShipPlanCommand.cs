using GymMgmt.Application.Common.Interfaces;
using GymMgmt.Application.Features.Memberships.GetMemberShipPlanById;

namespace GymMgmt.Application.Features.Memberships.CreateMembership
{
    public sealed record CreateMemberShipPlanCommand(
        string Name,
        int DurationInMonths,
        decimal Price,
        bool IsActive
        ):ICommand<ReadMemberShipPlanDto>;
}
