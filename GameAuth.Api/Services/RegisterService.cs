using GameAuth.Api.Services.Interface;
using GameAuth.Api.Models.Dto;
using GameAuth.Api.Models.Dto.Register;
using GameAuth.Api.Mappers;
using GameAuth.Api.Validators;
using GameAuth.Database.Repository.Interface;
using System.Security.Claims;

namespace GameAuth.Api.Services;

public class RegisterService : IRegisterService
{
    private readonly IAccountValidator accountValidator;
    private readonly IAccountRepository accountRepository;
    private readonly IHashingService hashingService;
    private readonly IJwtService jwtService;
    private readonly IConfirmationEmailService confirmationEmailService;

    public RegisterService(
        IAccountValidator accountValidator,
        IAccountRepository accountRepository,
        IHashingService hashingService,
        IJwtService jwtService,
        IConfirmationEmailService confirmationEmailService)
    {
        this.accountValidator = accountValidator;
        this.accountRepository = accountRepository;
        this.hashingService = hashingService;
        this.jwtService = jwtService;
        this.confirmationEmailService = confirmationEmailService;
    }

    public async Task<AuthResponse<TokenResponse>> Register(RegisterRequest request)
    {
        // Handle validation of request and setup response for later
        var res = new AuthResponse<TokenResponse>();
        var errors = accountValidator.FullRegisterValidation(request);
        if (errors.Any())
        {
            res.Errors.AddRange(errors);
            return res;
        }

        // Generate password hash/salt
        var passwordHash = hashingService.HashPassword(request.Password, out var passwordSalt);

        // Make sure password is never read again
        request.Password = string.Empty;
        request.MatchingPassword = string.Empty;

        // Map validated request to an Account entity and add this entity to the database
        var mappedAccount = AccountMapper.MapValidatedRegisterRequestToAccount(request, passwordHash, passwordSalt);

        // Verify email is unique (may need to add verified flag to this check)
        var emailExsists = await accountRepository.EmailExisits(request.Email);
        if (emailExsists)
        {
            res.Errors.Add("email is not unique");
            return res;
        }

        // Register account in database
        var registeredAccount = await accountRepository.RegisterAccount(mappedAccount);
        if (registeredAccount is null)
        {
            res.Errors.Add("registration was not completed");
            return res;
        }

        // Send verification email
        await confirmationEmailService.SendVerificationEmail(registeredAccount);

        // Return JWT tokens
        res.Data = jwtService.CreateJwtSet(registeredAccount);
        return res;
    }

    public async Task<AuthResponse<bool>> ResendVerificationEmail(ClaimsIdentity identity)
    {
        var res = new AuthResponse<bool>();
        var accountIdClaim = identity.Claims.FirstOrDefault(c => c.Type.Equals("AccountId"));
        var parsed = long.TryParse(accountIdClaim?.Value, out var accountId);

        if (!parsed)
        {
            throw new Exception("Failed to parse accountId from claims");
        }

        var account = await accountRepository.GetAccountById(accountId);

        if (account is null)
        {
            throw new Exception("Failed to find account based on accountId in claims");
        }

        var code = await confirmationEmailService.SendVerificationEmail(account);

        if (string.IsNullOrWhiteSpace(code))
        {
            throw new NullReferenceException("Failed to properly send verification email");
        }

        res.Data = true;
        return res;
    }
}