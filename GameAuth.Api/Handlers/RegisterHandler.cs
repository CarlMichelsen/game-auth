using GameAuth.Api.Handlers.Interface;
using GameAuth.Database.Repository.Interface;
using GameAuth.Api.Services.Interface;

using GameAuth.Api.Models.Dto;
using GameAuth.Api.Models.Dto.Register;
using GameAuth.Api.Mappers;
using GameAuth.Api.Validators;

namespace GameAuth.Api.Handlers;

public class RegisterHandler : IRegisterHandler
{
    private readonly IAccountValidator accountValidator;
    private readonly IAccountRepository accountRepository;
    private readonly IHashingService hashingService;
    private readonly IJwtService jwtService;
    private readonly IConfirmationEmailService confirmationEmailService;

    public RegisterHandler(
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

        // Map validated request to an Account entity
        var mappedAccount = AccountMapper.MapValidatedRegisterRequestToAccount(request, passwordHash, passwordSalt);

        // Verify email is unique (may need to only check for exsisting verified emails)
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
        await confirmationEmailService.SendVerificationEmail(
            registeredAccount,
            registeredAccount.Email);

        // Return JWT tokens
        res.Data = jwtService.CreateIdentityToken(registeredAccount);
        return res;
    }
}