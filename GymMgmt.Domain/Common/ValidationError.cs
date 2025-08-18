using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymMgmt.Domain.Common
{
    public static class ValidationError
    {
        public static Error Required(string field) =>
            new Error(
                Code: $"REQUIRED_{field.ToUpper()}",
                Message: $"'{field}' is required.");

        public static Error InvalidFormat(string field) =>
            new Error(
                Code: $"INVALID_FORMAT_{field.ToUpper()}",
                Message: $"'{field}' has an invalid format.");

        public static Error MustBePositive(string field) =>
            new Error(
                Code: "MUST_BE_POSITIVE",
                Message: $"'{field}' must be positive.");

        public static Error MustBeInRange(string field, int min, int max) =>
            new Error(
                Code: "OUT_OF_RANGE",
                Message: $"'{field}' must be between {min} and {max}.");
    }
}
