using GameAuth.Api.Services.Interface;
using GameAuth.Api.Models.Dto;
using GameAuth.Api.Models.Dto.Login;
using GameAuth.Api.Validators;
using GameAuth.Database.Repository.Interface;

namespace GameAuth.Api.Services;

public class LoginService : ILoginService
{
    private readonly IAccountRepository accountRepository;
    private readonly IHashingService hashingService;
    private readonly IAccountValidator accountValidator;
    private readonly IJwtService jwtService;
    private readonly string loginFailedString = "login failed";

    public LoginService(
        IAccountRepository accountRepository,
        IHashingService hashingService,
        IAccountValidator accountValidator,
        IJwtService jwtService)
    {
        this.accountRepository = accountRepository;
        this.hashingService = hashingService;
        this.accountValidator = accountValidator;
        this.jwtService = jwtService;
    }

    public async Task<AuthResponse<TokenResponse>> Login(LoginRequest request)
    {
        // Make sure the email is valid and create a response for later
        var res = new AuthResponse<TokenResponse>();
        var errors = accountValidator.Email(request.Email);
        if (errors.Any())
        {
            res.Errors.AddRange(errors);
            return res;
        }

        // Find account linked to email
        var account = await accountRepository.GetAccountByEmail(request.Email);
        if (account is null)
        {
            res.Errors.Add(loginFailedString);
            return res;
        }

        // Match password
        var validPassword = hashingService.VerifyPassword(request.Password, account.PasswordHash, account.PasswordSalt);
        if (!validPassword)
        {
            res.Errors.Add(loginFailedString);
            return res;
        }

        // Placeholder TokenResponse
        res.Data = jwtService.CreateJwtSet(account);
        return res;
    }
}