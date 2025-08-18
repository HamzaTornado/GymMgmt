using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymMgmt.Domain.Exceptions
{
    public sealed class SubscriptionPaymentAmountNotPositiveException : DomainException
    {
        public SubscriptionPaymentAmountNotPositiveException(decimal amount)
            : base(
                errorCode: "SUBSCRIPTION_PAYMENT_AMOUNT_NOT_POSITIVE",
                message: $"Payment amount {amount} must be greater than zero.")
        { }
    }
}
