using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymMgmt.Domain.Exceptions
{
    public sealed class InsuranceFeeRequiredException : DomainException
    {
        public InsuranceFeeRequiredException()
            : base(
                errorCode: "INSURANCE_FEE_REQUIRED",
                message: "Initial insurance fee is required to create ClubSettings.")
        { }
    }
}
