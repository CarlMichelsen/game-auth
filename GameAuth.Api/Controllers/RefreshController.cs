using GameAuth.Api.Models.Dto;
using GameAuth.Api.Services.Interface;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace GameAuth.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class RefreshController : ControllerBase
{
    private readonly ILogger<RefreshController> logger;
    private readonly IRefreshService refreshService;

    public RefreshController(ILogger<RefreshController> logger, IRefreshService refreshService)
    {
        this.logger = logger;
        this.refreshService = refreshService;
    }

    [Authorize]
    [HttpPost]
    public async Task<ActionResult<AuthResponse<TokenResponse>>> Refresh()
    {
        try
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            var refreshToken = await HttpContext.GetTokenAsync("access_token")
                ?? throw new NullReferenceException("Token should exsist when this point is reached");

            var res = await refreshService.Refresh(identity, refreshToken);
            var accountId = identity?.Claims.FirstOrDefault(a => a.Type.Equals("AccountId"))?.Value ?? "<No AccountId>";
            if (res.Ok)
            {
                logger.LogInformation(
                    "{} refreshed their token",
                    accountId
                );
                return Ok(res);
            }
            else
            {
                logger.LogWarning(
                    "{} failed to refresh their token",
                    accountId
                );
                return Unauthorized(res);
            }
        }
        catch (Exception e)
        {
            logger.LogCritical(
                "A token refresh attempt caused an internal server error while attempting login: \"{}\" [{}]",
                e.Message,
                e.Source
            );
            Response.StatusCode = 500;
            return Content("Internal server error");
        }
    }
}