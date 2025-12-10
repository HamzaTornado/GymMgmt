using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymMgmt.Application.Features.Members.Queries.GetMemberStatistics
{
    public record GetMemberStatisticsQuery : IRequest<MemberStatisticsDto>;
}
