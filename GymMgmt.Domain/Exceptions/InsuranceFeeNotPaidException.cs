using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymMgmt.Domain.Exceptions
{
    public class InsuranceFeeNotPaidException : DomainException
    {
        public InsuranceFeeNotPaidException(Guid memberId) : base(
            errorCode: "INSURANCE_FEE_NOT_PAID",
            message: $"Cannot start a subscription until the insurance fee for member {memberId} is paid.")
                { }
    }
}
