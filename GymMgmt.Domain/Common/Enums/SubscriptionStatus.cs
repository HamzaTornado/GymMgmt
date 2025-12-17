using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymMgmt.Domain.Common.Enums
{
    public enum SubscriptionStatus
    {
        Active =1,
        Grace = 2,
        Expired = 3,
        Cancelled = 4,
        Revoked = 5
    }
}
