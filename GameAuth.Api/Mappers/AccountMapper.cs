using GameAuth.Api.Models.Dto.Register;
using GameAuth.Database.Models.Entities;

namespace GameAuth.Api.Mappers;

public static class AccountMapper
{
    public static Account MapValidatedRegisterRequestToAccount(RegisterRequest request, string passwordHash, byte[] passwordSalt)
    {
        var now = DateTime.UtcNow;

        var email = new Email
        {
            Value = request.Email,
            Added = now,
            IsPrimary = true,
            LastSetToPrimary = now
        };

        var address = new Address
        {
            CountryOrRegion = request.Address.CountryOrRegion,
            AddressLine1 = request.Address.AddressLine1,
            AddressLine2 = request.Address.AddressLine2,
            City = request.Address.City,
            StateProvinceOrRegion = request.Address.StateProvinceOrRegion,
            ZipOrPostalCode = request.Address.ZipOrPostalCode,
            LastModified = now
        };

        return new Account
        {
            Emails = new List<Email> { email },
            EmailVerified = false,
            FullName = request.FullName,
            PhoneNumber = request.PhoneNumber,
            PasswordHash = passwordHash,
            PasswordSalt = passwordSalt,
            Address = address,
            LastModified = now
        };
    }
}