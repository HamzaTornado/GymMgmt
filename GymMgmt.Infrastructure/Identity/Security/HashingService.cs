using GymMgmt.Infrastructure.Exceptions;
using System.Security.Cryptography;
using System.Text;


namespace GymMgmt.Infrastructure.Identity.Security
{
    public static class HashingService
    {
        private const int SaltSize = 16; // 128 bits
        private const int HashSize = 32; // 256 bits
        private const int Iterations = 100_000;

        public static string Hash(string input)
        {
            if (string.IsNullOrEmpty(input))
                throw new HashInputEmptyException();

            Span<byte> salt = stackalloc byte[SaltSize];
            RandomNumberGenerator.Fill(salt);

            byte[] hash = Rfc2898DeriveBytes.Pbkdf2(
                Encoding.UTF8.GetBytes(input),
                salt.ToArray(),
                Iterations,
                HashAlgorithmName.SHA256,
                HashSize);

            byte[] result = new byte[SaltSize + HashSize];
            salt.CopyTo(result);
            hash.CopyTo(result.AsSpan(SaltSize));

            return Convert.ToBase64String(result);
        }

        public static bool Verify(string input, string storedHash)
        {
            if (string.IsNullOrEmpty(input) || string.IsNullOrEmpty(storedHash))
                return false;

            byte[] hashBytes = Convert.FromBase64String(storedHash);
            if (hashBytes.Length != SaltSize + HashSize)
                return false;

            Span<byte> salt = hashBytes.AsSpan(0, SaltSize);
            byte[] expectedHash = hashBytes.AsSpan(SaltSize).ToArray();

            byte[] computed = Rfc2898DeriveBytes.Pbkdf2(
                Encoding.UTF8.GetBytes(input),
                salt.ToArray(),
                Iterations,
                HashAlgorithmName.SHA256,
                HashSize);

            return CryptographicOperations.FixedTimeEquals(computed, expectedHash);
        }
    }
}
