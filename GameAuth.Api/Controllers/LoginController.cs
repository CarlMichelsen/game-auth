using GameAuth.Api.Handlers.Interface;
using GameAuth.Api.Models.Dto;
using GameAuth.Api.Models.Dto.Login;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GameAuth.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class LoginController : ControllerBase
{
    private readonly ILogger<LoginController> logger;
    private readonly ILoginHandler loginHandler;

    public LoginController(ILogger<LoginController> logger, ILoginHandler loginHandler)
    {
        this.logger = logger;
        this.loginHandler = loginHandler;
    }

    [AllowAnonymous]
    [HttpPost]
    public async Task<ActionResult<AuthResponse<TokenResponse>>> Login([FromBody] LoginRequest request)
    {
        try
        {
            var res = await loginHandler.Login(request);
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