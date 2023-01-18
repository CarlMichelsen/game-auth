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

    public RegisterController(
        ILogger<RegisterController> logger,
        IRegisterService registerService
    )
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
            return Content("Internal server error");
        }
    }
}