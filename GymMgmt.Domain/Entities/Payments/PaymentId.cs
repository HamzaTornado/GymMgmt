using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymMgmt.Domain.Entities.Payments
{
    public record PaymentId(Guid Value)
    {
        public static PaymentId New() => new(Guid.NewGuid());
        public override string ToString() => Value.ToString();
    }
}
