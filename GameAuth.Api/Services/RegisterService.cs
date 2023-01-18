using GameAuth.Api.Services.Interface;
using GameAuth.Api.Models.Dto;
using GameAuth.Api.Models.Dto.Register;
using GameAuth.Api.Mappers;
using GameAuth.Api.Validators;
using GameAuth.Database.Repository.Interface;

namespace GameAuth.Api.Services;

public class RegisterService : IRegisterService
{
    private readonly IAccountValidator accountValidator;
    private readonly IAccountRepository accountRepository;
    private readonly IEmailRepository emailRepository;
    private readonly IHashingService hashingService;
    private readonly IJwtService jwtService;

    public RegisterService(
        IAccountValidator accountValidator,
        IAccountRepository accountRepository,
        IEmailRepository emailRepository,
        IHashingService hashingService,
        IJwtService jwtService)
    {
        this.accountValidator = accountValidator;
        this.accountRepository = accountRepository;
        this.emailRepository = emailRepository;
        this.hashingService = hashingService;
        this.jwtService = jwtService;
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
        var exsistingAccountWithEmail = await emailRepository.GetPrimaryEmail(request.Email);
        if (exsistingAccountWithEmail is not null)
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

        // Return JWT tokens
        res.Data = jwtService.CreateJwtSet(registeredAccount);
        return res;
    }
}