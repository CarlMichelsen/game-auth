using GameAuth.Api.Services.Interface;
using System.Security.Cryptography;
using System.Text;

namespace GameAuth.Api.Services;

public class HashingService : IHashingService
{
    private const int keySize = 64;
    private const int iterations = 350000;
    private readonly HashAlgorithmName hashAlgorithm = HashAlgorithmName.SHA512;

    public string HashPassword(string password, out byte[] salt)
    {
        salt = RandomNumberGenerator.GetBytes(keySize);

        var hash = Rfc2898DeriveBytes.Pbkdf2(
            Encoding.UTF8.GetBytes(password),
            salt,
            iterations,
            hashAlgorithm,
            keySize
        );

        return Convert.ToHexString(hash);
    }

    public bool VerifyPassword(string password, string hash, byte[] salt)
    {
        var hashToCompare = Rfc2898DeriveBytes.Pbkdf2(
            password,
            salt,
            iterations,
            hashAlgorithm,
            keySize
        );
        var hashAsByteArr = Convert.FromHexString(hash);

        return hashToCompare.SequenceEqual(hashAsByteArr);
    }
}