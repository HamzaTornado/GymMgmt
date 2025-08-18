using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymMgmt.Domain.Exceptions
{
    public sealed class MembershipPlanPriceNegativeException : DomainException
    {
        public decimal Price { get; }

        public MembershipPlanPriceNegativeException(decimal price)
            : base(
                errorCode: "MEMBERSHIP_PLAN_PRICE_NEGATIVE",
                message: $"Price cannot be negative. Received: {price}.")
        {
            Price = price;
        }
    }
}
