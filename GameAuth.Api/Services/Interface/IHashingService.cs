namespace GameAuth.Api.Services.Interface;

public interface IHashingService
{
    string HashPassword(string password, out byte[] salt);
    bool VerifyPassword(string password, string hash, byte[] salt);
}