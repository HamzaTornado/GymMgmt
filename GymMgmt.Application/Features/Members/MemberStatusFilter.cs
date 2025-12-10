using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymMgmt.Application.Features.Members
{
    public enum MemberStatusFilter
    {
        Active = 1,
        GracePeriod = 2,
        Expired = 3,
        Cancelled = 4
    }
}
