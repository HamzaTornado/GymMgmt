using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymMgmt.Domain.Common.Enums
{
    public enum PaymentStatus
    {
        Paid,
        Pending,
        Unpaid,
        Refunded,
        Failed
    }
}
