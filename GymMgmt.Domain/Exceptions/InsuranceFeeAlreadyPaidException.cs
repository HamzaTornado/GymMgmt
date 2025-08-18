using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymMgmt.Domain.Exceptions
{
    public class InsuranceFeeAlreadyPaidException(string memberFullName) : DomainException(
        errorCode: "INSURANCE_FEE_ALREADY_PAID",
        message: $"The member with Id {memberFullName} has already paid the insurance fee.")
    {
    }
}
