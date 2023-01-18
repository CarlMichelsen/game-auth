using System.Text.RegularExpressions;
using GameAuth.Api.Models.Dto;
using GameAuth.Api.Models.Dto.Register;

namespace GameAuth.Api.Validators;

public partial class AccountValidator : IAccountValidator
{

    private readonly Regex emailRegex = EmailRegex();
    private readonly Regex specialCharactersRegex = SpecialCharactersRegex();
    private readonly Regex numberCharatersRegex = NumberCharactersRegex();
    private readonly Regex phoneNumberRegex = PhoneNumberRegex();

    private IEnumerable<string> CountryOrRegion(string countryOrRegion)
    {
        var maxLength = 255;
        var minLength = 1;
        var errors = new List<string>();

        if (string.IsNullOrWhiteSpace(countryOrRegion))
        {
            errors.Add("no countryOrRegion");
            return errors;
        }

        if (countryOrRegion.Length < minLength)
        {
            errors.Add($"countryOrRegion can't be shorter than {minLength} characters");
        }

        if (countryOrRegion.Length > maxLength)
        {
            errors.Add($"countryOrRegion can't be longer than {maxLength} characters");
        }

        if (specialCharactersRegex.IsMatch(countryOrRegion))
        {
            errors.Add("countryOrRegion can't contain special characters");
        }

        return errors;
    }

    private static IEnumerable<string> AddressLine(string addressLine, string end = "")
    {
        var maxLength = 255;
        var minLength = 1;
        var errors = new List<string>();

        if (string.IsNullOrWhiteSpace(addressLine))
        {
            errors.Add($"no addressLine{end}");
            return errors;
        }

        if (addressLine.Length < minLength)
        {
            errors.Add($"addressLine{end} can't be shorter than {minLength} characters");
        }

        if (addressLine.Length > maxLength)
        {
            errors.Add($"addressLine{end} can't be longer than {maxLength} characters");
        }

        return errors;
    }

    private static IEnumerable<string> City(string city)
    {
        var maxLength = 255;
        var minLength = 3;
        var errors = new List<string>();

        if (string.IsNullOrWhiteSpace(city))
        {
            errors.Add("no city name");
            return errors;
        }

        if (city.Length < minLength)
        {
            errors.Add($"city name can't be shorter than {minLength} characters");
        }

        if (city.Length > maxLength)
        {
            errors.Add($"city name can't be longer than {maxLength} characters");
        }

        return errors;
    }

    private IEnumerable<string> StateProvinceOrRegion(string stateProvinceOrRegion)
    {
        var maxLength = 255;
        var minLength = 1;
        var errors = new List<string>();

        if (string.IsNullOrWhiteSpace(stateProvinceOrRegion))
        {
            errors.Add("no stateProvinceOrRegion");
            return errors;
        }

        if (stateProvinceOrRegion.Length < minLength)
        {
            errors.Add($"stateProvinceOrRegion can't be shorter than {minLength} characters");
        }

        if (stateProvinceOrRegion.Length > maxLength)
        {
            errors.Add($"stateProvinceOrRegion can't be longer than {maxLength} characters");
        }

        if (specialCharactersRegex.IsMatch(stateProvinceOrRegion))
        {
            errors.Add("stateProvinceOrRegion can't contain special characters");
        }

        return errors;
    }

    private static IEnumerable<string> ZipOrPostalCode(string zipOrPostalCode)
    {
        var maxLength = 255;
        var minLength = 1;
        var errors = new List<string>();

        if (string.IsNullOrWhiteSpace(zipOrPostalCode))
        {
            errors.Add("no zipOrPostalCode");
            return errors;
        }

        if (zipOrPostalCode.Length < minLength)
        {
            errors.Add($"zipOrPostalCode can't be shorter than {minLength} characters");
        }

        if (zipOrPostalCode.Length > maxLength)
        {
            errors.Add($"zipOrPostalCode can't be longer than {maxLength} characters");
        }

        return errors;
    }

    public IEnumerable<string> Address(Address address)
    {
        var errors = new List<string>();

        errors.AddRange(CountryOrRegion(address.CountryOrRegion));
        errors.AddRange(AddressLine(address.AddressLine1, "1"));
        if (!string.IsNullOrWhiteSpace(address.AddressLine2)) errors.AddRange(AddressLine(address.AddressLine2, "2"));
        errors.AddRange(City(address.City));
        errors.AddRange(StateProvinceOrRegion(address.StateProvinceOrRegion));
        errors.AddRange(ZipOrPostalCode(address.ZipOrPostalCode));

        return errors;
    }

    public IEnumerable<string> Email(string email)
    {
        var maxLength = 254;
        var errors = new List<string>();
        if (string.IsNullOrWhiteSpace(email))
        {
            errors.Add("no email");
            return errors;
        }

        if (!email.Contains('@'))
        {
            errors.Add("no '@' symbol");
        }

        if (!email.Contains('.'))
        {
            errors.Add("no '.' symbol");
        }

        if (email.Length > maxLength)
        {
            errors.Add($"email can't be longer than {maxLength} characters");
            return errors;
        }

        if (!emailRegex.IsMatch(email))
        {
            errors.Add("invalid email");
        }

        return errors;
    }

    public IEnumerable<string> EmailMatch(string email1, string email2)
    {
        var errors = new List<string>();
        errors = errors.Concat(Email(email1)).ToList();
        if (!email1.Equals(email2))
        {
            errors.Add("emails don't match");
        }

        return errors;
    }

    public IEnumerable<string> FullName(string fullName)
    {
        var maxLength = 255;
        var errors = new List<string>();
        if (string.IsNullOrWhiteSpace(fullName))
        {
            errors.Add("no fullName");
            return errors;
        }

        if (fullName.Length > maxLength)
        {
            errors.Add($"fullName can't be longer than {maxLength} characters");
        }

        if (specialCharactersRegex.IsMatch(fullName))
        {
            errors.Add("fullName can't contain special characters");
        }

        return errors;
    }

    private IEnumerable<string> Password(string password)
    {
        var maxLength = 255;
        var minLength = 6;
        var errors = new List<string>();

        if (string.IsNullOrWhiteSpace(password))
        {
            errors.Add("no password");
            return errors;
        }

        if (password.Length < minLength)
        {
            errors.Add($"password can't be shorter than {minLength} characters");
        }

        if (password.Length > maxLength)
        {
            errors.Add($"password can't be longer than {maxLength} characters");
        }

        if (!specialCharactersRegex.IsMatch(password))
        {
            errors.Add($"password must contain at least one special character");
        }

        if (!numberCharatersRegex.IsMatch(password))
        {
            errors.Add($"password must contain at least one number");
        }

        return errors;
    }

    public IEnumerable<string> PasswordMatch(string password1, string password2)
    {
        var errors = new List<string>();
        errors = errors.Concat(Password(password1)).ToList();
        if (!password1.Equals(password2))
        {
            errors.Add("passwords don't match");
        }

        return errors;
    }

    public IEnumerable<string> PhoneNumber(string phoneNumber)
    {
        var maxLength = 255;
        var minLength = 6;
        var errors = new List<string>();
        if (string.IsNullOrWhiteSpace(phoneNumber))
        {
            errors.Add("no phoneNumber");
            return errors;
        }

        if (phoneNumber.Length < minLength)
        {
            errors.Add($"phoneNumber must be longer than {minLength} characters");
        }

        if (phoneNumber.Length > maxLength)
        {
            errors.Add($"phoneNumber must be shorter than {maxLength} characters");
        }

        if (!phoneNumberRegex.IsMatch(phoneNumber))
        {
            errors.Add("invalid phoneNumber");
        }

        return errors;
    }

    public IEnumerable<string> FullRegisterValidation(RegisterRequest request)
    {
        var address = Address(request.Address);
        var email = EmailMatch(request.Email, request.MatchingEmail);
        var fullName = FullName(request.FullName);
        var password = PasswordMatch(request.Password, request.MatchingPassword);
        var phoneNumber = PhoneNumber(request.PhoneNumber);

        var errors = new List<string>();


        errors.AddRange(address.Select(e => $"address: {e}"));
        errors.AddRange(email.Select(e => $"email: {e}"));
        errors.AddRange(fullName.Select(e => $"fullName: {e}"));
        errors.AddRange(password.Select(e => $"password: {e}"));
        errors.AddRange(phoneNumber.Select(e => $"phoneNumber: {e}"));

        return errors;
    }

    [GeneratedRegex("""^([\w\-\.]+)@((\[([0-9]{1,3}\.){3}[0-9]{1,3}\])|(([\w\-]+\.)+)([a-zA-Z]{2,4}))$""", RegexOptions.Compiled)]
    private static partial Regex EmailRegex();

    [GeneratedRegex("""[$"&+,:;=?@#|'<>.^*()%!-]""", RegexOptions.Compiled)]
    private static partial Regex SpecialCharactersRegex();

    [GeneratedRegex("""[0-9]""", RegexOptions.Compiled)]
    private static partial Regex NumberCharactersRegex();

    [GeneratedRegex("""^((\(?\+45\)?)?)(\s?\d{2}\s?\d{2}\s?\d{2}\s?\d{2})$""", RegexOptions.Compiled)]
    private static partial Regex PhoneNumberRegex();
}