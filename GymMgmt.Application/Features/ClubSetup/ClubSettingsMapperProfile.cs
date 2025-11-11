using AuthSystem.Application.Features.Memebers;
using AutoMapper;
using GymMgmt.Application.Features.ClubSetup.GetClubSettings;
using GymMgmt.Application.Features.Members.GetMemberById;
using GymMgmt.Domain.Common.ValueObjects;
using GymMgmt.Domain.Entities.ClubSettingsConfig;
using GymMgmt.Domain.Entities.Members;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymMgmt.Application.Features.ClubSetup
{
    public class ClubSettingsMapperProfile :Profile
    {
        public ClubSettingsMapperProfile() {
            
            CreateMap<ClubSettingsId, Guid>().ConvertUsing(src => src.Value);
            CreateMap<Guid, ClubSettingsId>().ConvertUsing(src => ClubSettingsId.FromValue(src));

            
            CreateMap<InsuranceFee, InsuranceFeeDto>();
            CreateMap<InsuranceFeeDto, InsuranceFee>();


            CreateMap<ClubSettings, ReadClubSettingsDto>()
            .ConstructUsing((src, context) => new ReadClubSettingsDto(
                src.Id.Value,
                context.Mapper.Map<InsuranceFeeDto>(src.CurrentInsuranceFee), 
                src.AreNewMembersAllowed,
                src.IsInsuranceFeeRequired,
                src.SubscriptionGracePeriodInDays,
                src.InsuranceValidityInDays
            ));


            CreateMap<ReadClubSettingsDto, ClubSettings>()
                .ForMember(dest => dest.CurrentInsuranceFee, opt => opt.MapFrom(src => src.InsuranceFee));
        }
    }
}
