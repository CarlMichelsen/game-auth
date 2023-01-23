using System.Security.Claims;
using GameAuth.Api.Models.Dto;
using GameAuth.Api.Models.Dto.Register;
using GameAuth.Api.Services.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GameAuth.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class RegisterController : ControllerBase
{
    private readonly ILogger<RegisterController> logger;
    private readonly IRegisterService registerService;

    private readonly string internalError = "Internal server error";

    public RegisterController(
        ILogger<RegisterController> logger,
        IRegisterService registerService)
    {
        this.logger = logger;
        this.registerService = registerService;
    }

    [AllowAnonymous]
    [HttpPost]
    public async Task<ActionResult<AuthResponse<TokenResponse>>> Register([FromBody] RegisterRequest request)
    {
        try
        {
            var res = await registerService.Register(request);
            if (res.Ok)
            {
                logger.LogInformation(
                    "{} completed registration",
                    request.FullName
                );
                return Ok(res);
            }
            else
            {
                logger.LogInformation(
                    "{} failed registration",
                    request.FullName
                );
                return BadRequest(res);
            }
        }
        catch (Exception e)
        {
            logger.LogCritical(
                "{} caused an internal server error while attempting registration: \"{}\" [{}]",
                request.FullName,
                e.Message,
                e.Source
            );
            Response.StatusCode = 500;
            return Content(internalError);
        }
    }

    [Authorize]
    [Route("ResendVerificationEmail")]
    [HttpPost]
    public async Task<ActionResult<AuthResponse<bool>>> ResendVerificationEmail()
    {
        try
        {
            if (HttpContext.User.Identity is not ClaimsIdentity identity)
                throw new NullReferenceException("No identity supplied when attempting to resend verification email");

            var accountId = identity.Claims.Single(c => c.Type.Equals("AccountId")).Value;
            var res = await registerService.ResendVerificationEmail(identity);
            if (res.Ok)
            {
                logger.LogInformation(
                    "verification email was resent for account <{}>",
                    accountId
                );
                return Ok(res);
            }
            else
            {
                logger.LogInformation(
                    "<{}> failed verification email resend",
                    accountId
                );
                return BadRequest(res);
            }
        }
        catch (Exception e)
        {
            logger.LogCritical(
                "verification email resending caused an internal server error: \"{}\" [{}]",
                e.Message,
                e.Source
            );
            Response.StatusCode = 500;
            return Content(internalError);
        }
    }

    [Authorize]
    [Route("VerifyEmail/{code}")]
    [HttpGet]
    public async Task<ActionResult<AuthResponse<bool>>> VerifyEmail([FromRoute] string code)
    {
        try
        {
            if (HttpContext.User.Identity is not ClaimsIdentity identity)
                throw new NullReferenceException("No identity supplied when attempting to verify email");

            var accountId = identity.Claims.Single(c => c.Type.Equals("AccountId")).Value;
            var res = await registerService.VerifyEmail(identity, code);
            if (res.Ok)
            {
                logger.LogInformation(
                    "account <{}> was verified",
                    accountId
                );
                return Ok(res);
            }
            else
            {
                logger.LogInformation(
                    "account <{}> failed verification",
                    accountId
                );
                return BadRequest(res);
            }
        }
        catch (Exception e)
        {
            logger.LogCritical(
                "email verification attempt caused an internal server error: \"{}\" [{}]",
                e.Message,
                e.Source
            );
            Response.StatusCode = 500;
            return Content(internalError);
        }
    }
}