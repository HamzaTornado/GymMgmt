using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymMgmt.Domain.Exceptions
{
    public class OverlappingSubscriptionException : DomainException
    {
        public OverlappingSubscriptionException() : base(
            errorCode: "OVERLAPPING_SUBSCRIPTION",
            message: "Cannot have overlapping active subscriptions.")
        { 

        }
    }
}
