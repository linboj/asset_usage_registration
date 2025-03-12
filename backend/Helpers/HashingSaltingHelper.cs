using System.Security.Cryptography;
using System.Text;

namespace backend.Helpers;

/// <summary>
/// Helper class for hashing and salting passwords.
/// </summary>
public class HashingSaltingHelper
{
    private const int keySize = 64;
    private const int iterations = 350000;
    private static HashAlgorithmName hashAlgorithm = HashAlgorithmName.SHA512;

    /// <summary>
    /// Hashes a password using PBKDF2 with a generated salt.
    /// </summary>
    /// <param name="password">The password to hash.</param>
    /// <param name="salt">The generated salt.</param>
    /// <returns>The hashed password as a hexadecimal string.</returns>
    public static string HashPasword(string password, out byte[] salt)
    {
        // Generate a random salt
        salt = RandomNumberGenerator.GetBytes(keySize);

        // Hash the password with the salt using PBKDF2
        var hash = Rfc2898DeriveBytes.Pbkdf2(
            Encoding.UTF8.GetBytes(password),
            salt,
            iterations,
            hashAlgorithm,
            keySize);

        // Return the hashed password as a hexadecimal string
        return Convert.ToHexString(hash);
    }

    /// <summary>
    /// Verifies a password against a given hash and salt.
    /// </summary>
    /// <param name="password">The password to verify.</param>
    /// <param name="hash">The hash to compare against.</param>
    /// <param name="salt">The salt used to hash the password.</param>
    /// <returns>True if the password matches the hash, otherwise false.</returns>
    public static bool VerifyPassword(string password, string hash, byte[] salt)
    {
        // Hash the password with the provided salt using PBKDF2
        var hashToCompare = Rfc2898DeriveBytes.Pbkdf2(password, salt, iterations, hashAlgorithm, keySize);

        // Compare the computed hash with the provided hash
        return CryptographicOperations.FixedTimeEquals(hashToCompare, Convert.FromHexString(hash));
    }
}