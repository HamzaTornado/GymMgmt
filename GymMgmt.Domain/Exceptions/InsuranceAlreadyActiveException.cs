using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymMgmt.Domain.Exceptions
{
    public class InsuranceAlreadyActiveException : DomainException
    {
        public InsuranceAlreadyActiveException(Guid memberId,DateTime onDate) : base(
            errorCode: "INSURANCE_ALREADY_ACTIVE",
            message: $"Member {memberId} already has active insurance on {onDate:yyyy-MM-dd}.")
                { }
    }
}
