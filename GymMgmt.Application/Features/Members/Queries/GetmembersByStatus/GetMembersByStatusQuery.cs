using GymMgmt.Application.Common.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymMgmt.Application.Features.Members.Queries.GetmembersByStatus
{
    public record GetMembersByStatusQuery(
    MemberStatusFilter StatusFilter,
    int PageNumber = 1,
    int PageSize = 10
) : IRequest<PaginatedList<MemberListDto>>;
}
