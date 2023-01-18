using GameAuth.Api.Models.Dto;
using GameAuth.Api.Models.Dto.Register;

namespace GameAuth.Api.Validators;

public interface IAccountValidator
{
    public IEnumerable<string> Email(string email);
    public IEnumerable<string> EmailMatch(string email1, string email2);
    public IEnumerable<string> FullName(string fullName);
    public IEnumerable<string> PhoneNumber(string phoneNumber);
    public IEnumerable<string> PasswordMatch(string password1, string password2);
    public IEnumerable<string> Address(Address address);
    public IEnumerable<string> FullRegisterValidation(RegisterRequest request);
}