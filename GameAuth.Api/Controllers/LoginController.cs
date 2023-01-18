using GameAuth.Api.Models.Dto;
using GameAuth.Api.Models.Dto.Login;
using GameAuth.Api.Services.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GameAuth.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class LoginController : ControllerBase
{
    private readonly ILogger<LoginController> logger;
    private readonly ILoginService loginService;

    public LoginController(ILogger<LoginController> logger, ILoginService loginService)
    {
        this.logger = logger;
        this.loginService = loginService;
    }

    [AllowAnonymous]
    [HttpPost]
    public async Task<ActionResult<AuthResponse<TokenResponse>>> Login([FromBody] LoginRequest request)
    {
        try
        {
            var res = await loginService.Login(request);
            if (res.Ok)
            {
                logger.LogInformation(
                    "{} logged in",
                    request.Email
                );
                return Ok(res);
            }
            else
            {
                logger.LogWarning(
                    "{} failed a login attempt",
                    request.Email
                );
                return Unauthorized(res);
            }
        }
        catch (Exception e)
        {
            logger.LogCritical(
                "{} caused an internal server error while attempting login: \"{}\" [{}]",
                request.Email,
                e.Message,
                e.Source
            );
            Response.StatusCode = 500;
            return Content("Internal server error");
        }
    }
}