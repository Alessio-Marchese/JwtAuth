using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System.Security.Cryptography;

namespace JwtAuthService.Helpers;

public interface IPasswordHelper
{
    (byte[] hashedPassword, byte[] salt) HashPassword(string password);
    bool VerifyPassword(string password, byte[] storedHash, byte[] storedSalt);
}

public class PasswordHelper : IPasswordHelper
{
    public (byte[] hashedPassword, byte[] salt) HashPassword(string password)
    {
        var salt = GenerateSalt();

        var hashedPassword = HashPasswordWithSalt(password, salt);

        return (hashedPassword, salt);
    }
    public bool VerifyPassword(string password, byte[] storedHash, byte[] storedSalt)
    {
        var hashedPassword = HashPasswordWithSalt(password, storedSalt);
        return hashedPassword == storedHash;
    }

    #region PRIVATE
    private static byte[] GenerateSalt()
    {
        byte[] saltBytes = new byte[16];
        RandomNumberGenerator.Fill(saltBytes);
        return saltBytes;
    }

    private static byte[] HashPasswordWithSalt(string password, byte[] saltBytes)
    {
        var hashBytes = KeyDerivation.Pbkdf2(
            password: password,
            salt: saltBytes,
            prf: KeyDerivationPrf.HMACSHA256,
            iterationCount: 10000,
            numBytesRequested: 256 / 8);

        return hashBytes;
    }
    #endregion
}
