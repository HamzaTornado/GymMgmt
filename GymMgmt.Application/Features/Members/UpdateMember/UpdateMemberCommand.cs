using AuthSystem.Application.Features.Memebers;
using GymMgmt.Application.Common.Interfaces;
using GymMgmt.Application.Features.Members.GetMemberById;


namespace GymMgmt.Application.Features.Members.UpdateMember
{
    public sealed record UpdateMemberCommand(
        Guid MemberId,
        string FirstName,
        string LastName,
        string? Email,
        string PhoneNumber,
        AddressDto? Address
        ) : ICommand<ReadMemberDto>;
}
