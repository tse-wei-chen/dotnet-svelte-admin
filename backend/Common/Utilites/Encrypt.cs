using System.Security.Cryptography;

namespace backend.Common.Utilities
{
    public class Encrypt
    {
        private const int SaltSize = 16;
        private const int KeySize = 32;
        private const int Iterations = 100_000;
        private static readonly HashAlgorithmName HashAlgorithm = HashAlgorithmName.SHA256;

        public static string Hash(string input)
        {
            byte[] salt = RandomNumberGenerator.GetBytes(SaltSize);
            var hash = Rfc2898DeriveBytes.Pbkdf2(input, salt, Iterations, HashAlgorithm, KeySize);
            return $"{Iterations}.{Convert.ToBase64String(salt)}.{Convert.ToBase64String(hash)}";
        }

        public static bool Verify(string input, string hashedInput)
        {
            try
            {
                var parts = hashedInput.Split('.');
                if (parts.Length != 3)
                    return false;
                int iterations = int.Parse(parts[0]);
                byte[] salt = Convert.FromBase64String(parts[1]);
                byte[] hash = Convert.FromBase64String(parts[2]);
                var hashToCompare = Rfc2898DeriveBytes.Pbkdf2(input, salt, iterations, HashAlgorithm, hash.Length);
                return CryptographicOperations.FixedTimeEquals(hash, hashToCompare);
            }
            catch
            {
                return false;
            }
        }
    }
}