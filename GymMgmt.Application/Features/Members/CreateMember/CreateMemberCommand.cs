using AuthSystem.Application.Features.Memebers;
using GymMgmt.Application.Common.Interfaces;
using GymMgmt.Application.Features.Members.GetMemberById;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymMgmt.Application.Features.Members.CreateMember
{
    public sealed record CreateMemberCommand(
        string FirstName,
        string LastName,
        string? Email,
        string PhoneNumber,
        AddressDto? Address
        ):ICommand<ReadMemberDto>;
}
