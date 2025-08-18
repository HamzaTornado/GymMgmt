using GymMgmt.Domain.Common.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymMgmt.Domain.Exceptions
{
    public class PaymentNotRefundableException : DomainException
    {
        public PaymentNotRefundableException(Guid paymentId, PaymentStatus currentStatus)
            : base(
                errorCode: "PAYMENT_NOT_REFUNDABLE",
                message: $"Only paid payments can be refunded. Current status of payment {paymentId} is {currentStatus}.")
                { }
    }
}
