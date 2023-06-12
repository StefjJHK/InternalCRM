using System.IdentityModel.Tokens.Jwt;
using BIP.InternalCRM.Infrastructure;
using BIP.InternalCRM.WebIdentity.Users;
using BIP.InternalCRM.WebIdentity.Users.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace BIP.InternalCRM.WebIdentity.Authorization;

[Route("auth")]
public class AuthController : ApiControllerBase
{
    private readonly IUserService _userService;

    public AuthController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpPost("/token")]
    [ProducesResponseType(typeof(TokenDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> OnGetTokenAsync(
        [FromBody] UserCredentialsDto payload,
        [FromServices] IHttpContextAccessor httpContextAccessor,
        CancellationToken cancellationToken)
    {
        var result = await _userService.GenerateTokenAsync(
            payload.Username,
            payload.Password,
            cancellationToken);

        return result.Match<IActionResult>(
            value =>
            {
                var httpContext = httpContextAccessor.HttpContext!;
                httpContext.Response.Headers
                    .Add("Access-Control-Allow-Credentials", "true");

                httpContext.Response.Cookies
                    .Append(
                        "access_token",
                        $"{new JwtSecurityTokenHandler().WriteToken(value)}; HttpOnly; Secure; Path=/");

                return Ok(Mapper.Map<TokenDto>(value));
            },
            BadRequest);
    }
}