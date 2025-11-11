using GymMgmt.Application.Common.Interfaces;
using GymMgmt.Application.Features.Members.GetMemberById;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymMgmt.Application.Features.Members.GetAllMembers
{
    public sealed record GetAllMembersQuery():IQuery<IEnumerable<ReadMemberDto>>;
}
