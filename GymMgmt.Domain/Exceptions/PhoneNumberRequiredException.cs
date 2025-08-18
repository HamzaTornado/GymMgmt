using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymMgmt.Domain.Exceptions
{
    public sealed class PhoneNumberRequiredException : DomainException
    {
        public PhoneNumberRequiredException()
            : base(
                errorCode: "PHONE_NUMBER_REQUIRED",
                message: "Le numéro de téléphone est requis.")
        { }
    }
}
