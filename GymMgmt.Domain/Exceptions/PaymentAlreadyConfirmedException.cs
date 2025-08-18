using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymMgmt.Domain.Exceptions
{
    public class PaymentAlreadyConfirmedException : DomainException
    {
        public PaymentAlreadyConfirmedException(Guid paymentId)
            : base(
                errorCode: "PAYMENT_ALREADY_CONFIRMED",
                message: $"Payment {paymentId} has already been confirmed.")
                { }
    }
}
