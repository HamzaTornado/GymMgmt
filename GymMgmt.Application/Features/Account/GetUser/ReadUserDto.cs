using AuthSystem.Application.Features.Memebers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymMgmt.Application.Features.Account.GetUser
{
    public sealed record ReadUserDto(
        string Id,
        string FirstName,
        string LastName,
        string PhoneNumber,
        string Email,
        List<string> Roles
        );
}
