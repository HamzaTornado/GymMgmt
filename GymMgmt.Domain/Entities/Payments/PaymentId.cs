using GymMgmt.Domain.Entities.ClubSettingsConfig;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymMgmt.Domain.Entities.Payments
{
    public record PaymentId(Guid Value)
    {
        public static PaymentId FromValue(Guid value) => new(value);
        public static PaymentId New() => FromValue(Guid.NewGuid());
    }
}
