using AuthSystem.Application.Features.Memebers;
using GymMgmt.Application.Common.Interfaces;
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
        AddressDto? AddressDto
        ):ICommand<Guid>;
}
