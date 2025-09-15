using GymMgmt.Domain.Entities.Members;
using AutoMapper;
using GymMgmt.Domain.Common.ValueObjects;
using AuthSystem.Application.Features.Memebers;
using GymMgmt.Application.Features.Members.GetMemberById;


namespace GymMgmt.Application.Features.Members
{
    public class MemberMapperProfile : Profile
    {
        public MemberMapperProfile()
        {
            // For MemberId ↔ Guid conversion
            CreateMap<MemberId, Guid>().ConvertUsing(src => src.Value);
            CreateMap<Guid, MemberId>().ConvertUsing(src => MemberId.FromValue(src));

            // Address <-> AddressDto
            CreateMap<Address, AddressDto>();
            CreateMap<AddressDto, Address>();

            // Member -> ReadMemberDto
            CreateMap<Member, ReadMemberDto>()
                .ForMember(dest => dest.AddressDto, opt => opt.MapFrom(src => src.Address));

            // ReadMemberDto -> Member
            CreateMap<ReadMemberDto, Member>()
                .ForMember(dest => dest.Address, opt => opt.MapFrom(src => src.AddressDto));
        }
    }
}
