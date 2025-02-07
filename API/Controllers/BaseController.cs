namespace FirstRestAPI.Controllers;

using Common;
using Common.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[Authorize]
[ApiController]
public class
    BaseController : ControllerBase
{
    private string AuthHeader => HttpContext.Request.Headers.Authorization.ToString();

    protected int UserId => AuthHeader.GetUserId();
    protected Roles Role => AuthHeader.GetRole();
}