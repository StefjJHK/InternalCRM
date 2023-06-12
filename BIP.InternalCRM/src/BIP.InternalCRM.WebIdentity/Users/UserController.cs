using BIP.InternalCRM.Infrastructure;
using BIP.InternalCRM.WebIdentity.Permissions;
using BIP.InternalCRM.WebIdentity.Roles.Dtos;
using BIP.InternalCRM.WebIdentity.Users.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace BIP.InternalCRM.WebIdentity.Users;

[Route("users")]
public class UserController : ApiControllerBase
{
    private readonly IUserService _userService;

    public UserController(IUserService userService)
    {
        _userService = userService;
    }
    
    [HttpGet]
    [ProducesResponseType(typeof(UserDto[]), StatusCodes.Status200OK)]
    public async Task<IActionResult> OnGetAllAsync(CancellationToken cancellationToken)
    {
        var result = await _userService.GetAllUsersAsync(cancellationToken);
        
        var dtos = Mapper.Map<UserDto[]>(result);

        return Ok(dtos);
    }

    [HttpGet("{username}")]
    [ProducesResponseType(typeof(UserDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> OnGetAsync([FromRoute] string username)
    {
        var result = await _userService.GetByUsernameAsync(username);

        return result.Match<IActionResult>(
            user => Ok(Mapper.Map<UserDto>(user)),
            _ => NotFound());
    }

    [HttpGet("{username}/permissions")]
    [ProducesResponseType(typeof(PermissionSet), StatusCodes.Status200OK)]
    public async Task<IActionResult> OnGetPermissionsAsync([FromRoute] string username)
    {
        var result = await _userService.GetByUsernameAsync(username);

        return result.Match<IActionResult>(
            user => Ok(user.PermissionSet),
            _ => NotFound());
    }

    [HttpGet("{username}/roles")]
    [ProducesResponseType(typeof(RoleDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> OnGetRolesAsync(
        [FromRoute] string username,
        CancellationToken cancellationToken)
    {
        var result = await _userService.GetUserRolesAsync(username, cancellationToken);

        var dtos = Mapper.Map<RoleDto[]>(result);

        return Ok(dtos);
    }
    
    [HttpPost]
    [ProducesResponseType(typeof(UserDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> OnAddAsync(
        [FromBody] UserAddDto payload,
        CancellationToken cancellationToken)
    {
        var result = await _userService.AddUserAsync(
            payload.Username,
            payload.Password,
            payload.Permissions);

        return result.Match<IActionResult>(
            user => Ok(Mapper.Map<UserDto>(user)),
            BadRequest);
    }
    
    [HttpPut("{username}")]
    [ProducesResponseType(typeof(UserDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> OnUpdateAsync(
        [FromRoute(Name = "username")] string usernameIdentity,
        [FromBody] UserUpdateDto payload)
    {
        var result = await _userService.UpdateAsync(
            usernameIdentity,
            payload.Username,
            payload.PermissionSet);

        return result.Match<IActionResult>(
            _ => Ok(),
            NotFound,
            BadRequest);
    }
    
    [HttpPut("{username}/password")]
    [ProducesResponseType(typeof(UserDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> OnChangePasswordAsync(
        [FromRoute(Name = "username")] string usernameIdentity,
        [FromBody] UserChangePasswordDto payload)
    {
        var result = await _userService.ChangePassword(
            usernameIdentity,
            payload.OldPassword,
            payload.NewPassword);

        return result.Match<IActionResult>(
            _ => Ok(),
            NotFound,
            BadRequest);
    }
    
    [HttpDelete("{username}")]
    [ProducesResponseType(typeof(UserDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> OnDeleteAsync([FromRoute] string username)
    {
        var result = await _userService.DeleteByUsernameAsync(username);

        return result.Match<IActionResult>(
            _ => Ok(),
            NotFound,
            BadRequest);
    }
}
