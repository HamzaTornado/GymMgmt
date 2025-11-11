using AutoMapper;
using GymMgmt.Application.Features.Members.GetMemberById;
using GymMgmt.Application.Features.Memberships.GetMemberShipPlanById;
using GymMgmt.Domain.Entities.Plans;


namespace GymMgmt.Application.Features.Memberships
{
    internal class MemberShipPlanMapperProfile : Profile
    {
        public MemberShipPlanMapperProfile() {

            CreateMap<MembershipPlanId, Guid>().ConvertUsing(src => src.Value);
            CreateMap<Guid, MembershipPlanId>().ConvertUsing(src => MembershipPlanId.FromValue(src));

            CreateMap<MembershipPlan, ReadMemberShipPlanDto>();
            CreateMap<ReadMemberShipPlanDto, MembershipPlan>();
        }
    }
}
