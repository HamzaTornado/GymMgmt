using GymMgmt.Domain.Entities.Members;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymMgmt.Domain.Entities.Subsciptions
{
    public record SubscriptionId(Guid Value)
    {
        public static SubscriptionId FromValue(Guid value) => new(value);
        public static SubscriptionId New() => FromValue(Guid.NewGuid());
    }
}
