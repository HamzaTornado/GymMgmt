using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymMgmt.Domain.Exceptions
{
    public sealed class InvalidMoroccanPhoneNumberException : DomainException
    {
        public string RawValue { get; }

        public InvalidMoroccanPhoneNumberException(string rawValue)
            : base(
                errorCode: "INVALID_MOROCCAN_PHONE_NUMBER",
                message: $"Numéro de téléphone marocain invalide : '{rawValue}'.")
        {
            RawValue = rawValue;
        }
    }
}
