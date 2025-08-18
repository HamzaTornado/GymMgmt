using System.Text.RegularExpressions;

namespace GymMgmt.Domain.Common.ValueObjects
{


    public sealed record PhoneNumber
    {
        private static readonly Regex _regex = new(
            @"^(?:\+212|0)(5|6|7)\d{8}$",
            RegexOptions.Compiled | RegexOptions.CultureInvariant);

        public string Value { get; }

        private PhoneNumber(string value)
        {
            Value = Normalize(value);
        }

        /// <summary>
        /// Creates a normalized and validated Moroccan phone number.
        /// </summary>
        /// <param name="value">Phone number in any valid format (e.g. 0612345678, +212612345678)</param>
        /// <returns>A valid PhoneNumber instance</returns>
        /// <exception cref="ArgumentException">Thrown if the number is invalid</exception>
        public static PhoneNumber Create(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("Le numéro de téléphone est requis.", nameof(value));

            // Clean the input: remove spaces, dashes, parentheses, etc.
            value = CleanPhoneNumber(value);

            if (!_regex.IsMatch(value))
                throw new ArgumentException("Numéro de téléphone marocain invalide.", nameof(value));

            return new PhoneNumber(value);
        }

        /// <summary>
        /// Cleans phone number input by removing common formatting characters
        /// </summary>
        private static string CleanPhoneNumber(string value)
        {
            return value.Trim()
                       .Replace(" ", "")
                       .Replace("-", "")
                       .Replace("(", "")
                       .Replace(")", "")
                       .Replace(".", "");
        }

        private static string Normalize(string value)
        {
            if (value.StartsWith("0"))
                return "+212" + value[1..]; // Remove '0', add '+212'
            if (!value.StartsWith("+212"))
                return "+212" + value;     // Prefix with country code
            return value;                  // Already in correct format
        }

        // Override ToString to return just the phone number
        public override string ToString() => Value;

        // Allow implicit conversion to string
        public static implicit operator string(PhoneNumber phone) => phone.Value;
    }
}
