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
            // For MemberId ↔ Guid conversion
            CreateMap<ClubSettingsId, Guid>().ConvertUsing(src => src.Value);
            CreateMap<Guid, ClubSettingsId>().ConvertUsing(src => ClubSettingsId.FromValue(src));

            // Address <-> AddressDto
            CreateMap<InsuranceFee, InsuranceFeeDto>();
            CreateMap<InsuranceFeeDto, InsuranceFee>();

            // Member -> ReadMemberDto
            CreateMap<ClubSettings, ReadClubSettingsDto>()
                .ForMember(dest => dest.InsuranceFeeDto, opt => opt.MapFrom(src => src.CurrentInsuranceFee));

            // ReadMemberDto -> Member
            CreateMap<ReadClubSettingsDto, ClubSettings>()
                .ForMember(dest => dest.CurrentInsuranceFee, opt => opt.MapFrom(src => src.InsuranceFeeDto));
        }
    }
}
