using GymMgmt.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymMgmt.Domain.Common.ValueObjects
{
    /// <summary>
    /// Represents a one-time insurance fee required when joining the gym.
    /// </summary>
    public record InsuranceFee
    {
        public decimal Amount { get; }
        public string Currency { get; } = "MAD"; // Moroccan Dirham
        public bool IsActive { get; } = true;

        public InsuranceFee(decimal amount, bool isActive = true)
        {
            Amount = amount;
            IsActive = isActive;
        }

        public static InsuranceFee Default() => new(100m); // e.g., 100 MAD

        public override string ToString()
        {
            var culture = CultureInfo.GetCultureInfo("fr-MA"); // French-Morocco formatting
            return $"{Amount.ToString("C", culture)} {Currency} (Insurance)";
        }

    }
}
