using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymMgmt.Application.Common.Models
{
    public sealed record UserInfo(
    string? Id,
    string? Email,
    string[] Roles
        );
}
