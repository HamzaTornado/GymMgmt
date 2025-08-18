using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymMgmt.Domain.Exceptions
{
    public sealed class InsuranceValidityNotPositiveException : DomainException
    {
        public int AttemptedValue { get; }

        public InsuranceValidityNotPositiveException(int days)
            : base(
                errorCode: "INSURANCE_VALIDITY_NOT_POSITIVE",
                message: $"Insurance validity must be positive. Received: {days} days.")
        {
            AttemptedValue = days;
        }
    }
}
