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
        var hash = ByteHashToCompare(password, salt);
        return Convert.ToHexString(hash);
    }

    public bool VerifyPassword(string password, string hash, byte[] salt)
    {
        var hashToCompare = ByteHashToCompare(password, salt);
        var hashAsByteArr = Convert.FromHexString(hash);
        return hashToCompare.SequenceEqual(hashAsByteArr);
    }

    private byte[] ByteHashToCompare(string password, byte[] salt)
    {
        return Rfc2898DeriveBytes.Pbkdf2(
            Encoding.UTF8.GetBytes(password),
            salt,
            iterations,
            hashAlgorithm,
            keySize
        );
    }
}