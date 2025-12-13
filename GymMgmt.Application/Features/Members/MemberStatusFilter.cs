using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymMgmt.Application.Features.Members
{
    public enum MemberStatusFilter
    {
        All = 1,
        Active = 2,
        GracePeriod = 3,
        Expired = 4,
        Cancelled = 5
    }
}
