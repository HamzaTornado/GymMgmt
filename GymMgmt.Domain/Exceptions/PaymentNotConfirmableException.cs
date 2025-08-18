using GymMgmt.Domain.Common.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymMgmt.Domain.Exceptions
{
    public class PaymentNotConfirmableException : DomainException
    {
        public PaymentNotConfirmableException(Guid paymentId, PaymentStatus currentStatus)
            : base(
                errorCode: "PAYMENT_NOT_CONFIRMABLE",
                message: $"Cannot confirm payment {paymentId} while its status is {currentStatus}.")
                { }
    }
}
