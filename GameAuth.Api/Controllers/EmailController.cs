using System.Security.Claims;
using GameAuth.Api.Handlers.Interface;
using GameAuth.Api.Models.Dto;
using GameAuth.Api.Models.Dto.Email;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GameAuth.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class EmailController : ControllerBase
{
    private readonly ILogger<EmailController> logger;
    private readonly IEmailHandler emailHandler;

    public EmailController(ILogger<EmailController> logger, IEmailHandler emailHandler)
    {
        this.logger = logger;
        this.emailHandler = emailHandler;
    }

    [Authorize]
    [Route("Verify/{code}")]
    [HttpPut]
    public async Task<ActionResult<AuthResponse<string>>> VerifyEmail([FromRoute] string code)
    {
        try
        {
            if (HttpContext.User.Identity is not ClaimsIdentity identity)
                throw new NullReferenceException("No identity supplied when attempting to resend verification email");

            var res = await emailHandler.VerifyEmail(identity, code);
            if (res.Ok)
            {
                logger.LogInformation(
                    "{} email was verified",
                    res.Data
                );
                return Ok(res);
            }
            else
            {
                logger.LogWarning(
                    "{} failed a verification attempt",
                    res.Data
                );
                return Unauthorized(res);
            }
        }
        catch (Exception e)
        {
            logger.LogCritical(
                "email verfication attempt with code {} caused an internal server error: \"{}\" [{}]",
                code,
                e.Message,
                e.Source
            );
            Response.StatusCode = 500;
            return Content("Internal server error");
        }
    }

    [Authorize]
    [Route("Resend")]
    [HttpPost]
    public async Task<ActionResult<AuthResponse<string>>> Resend([FromBody] ResendVerificationEmailRequest request)
    {
        try
        {
            if (HttpContext.User.Identity is not ClaimsIdentity identity)
                throw new NullReferenceException("No identity supplied when attempting to resend verification email");

            var res = await emailHandler.ResendVerificationEmail(identity, request.Email);
            if (res.Ok)
            {
                logger.LogInformation(
                    "{} verification email was resent",
                    res.Data
                );
                return Ok(res);
            }
            else
            {
                logger.LogWarning(
                    "{} verification email failed to resend",
                    request.Email
                );
                return Unauthorized(res);
            }
        }
        catch (Exception e)
        {
            logger.LogCritical(
                "resending verification email for {} caused an internal server error: \"{}\" [{}]",
                request.Email,
                e.Message,
                e.Source
            );
            Response.StatusCode = 500;
            return Content("Internal server error");
        }
    }
}