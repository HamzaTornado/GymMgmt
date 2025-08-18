using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymMgmt.Domain.Exceptions
{
    public sealed class InvalidInsuranceFeeAmountException : DomainException
    {
        public InvalidInsuranceFeeAmountException(decimal amount)
            : base(
                errorCode: "INSURANCE_FEE_AMOUNT_NOT_POSITIVE",
                message: $"Insurance Fee amount {amount} must be greater than zero.")
        { }
    }
}
