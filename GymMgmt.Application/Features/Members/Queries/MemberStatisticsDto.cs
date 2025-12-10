using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymMgmt.Application.Features.Members.Queries
{
    public record MemberStatisticsDto(
    int ActiveCount,
    int GracePeriodCount,
    int ExpiredCount,
    int CancelledCount
);
}
