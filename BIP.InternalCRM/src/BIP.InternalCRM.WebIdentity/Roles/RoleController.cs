using BIP.InternalCRM.Infrastructure;
using BIP.InternalCRM.WebIdentity.Roles.Dtos;
using BIP.InternalCRM.WebIdentity.Users;
using BIP.InternalCRM.WebIdentity.Users.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace BIP.InternalCRM.WebIdentity.Roles;

[Route("roles")]
public class RoleController : ApiControllerBase
{
    private readonly IRoleService _roleService;
    private readonly IUserService _userService;

    public RoleController(IRoleService roleService, IUserService userService)
    {
        _roleService = roleService;
        _userService = userService;
    }

    [HttpGet]
    [ProducesResponseType(typeof(RoleDto[]), StatusCodes.Status200OK)]
    public async Task<IActionResult> OnGetAllAsync(CancellationToken cancellationToken)
    {
        var result = await _roleService.GetAllRolesAsync(cancellationToken);

        var dtos = Mapper.Map<RoleDto[]>(result);

        return Ok(dtos);
    }

    [HttpGet("{roleName}")]
    [ProducesResponseType(typeof(RoleDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> OnGetAsync([FromRoute] string roleName)
    {
        var result = await _roleService.GetRoleByName(roleName);

        return result.Match<IActionResult>(
            role => Ok(Mapper.Map<RoleDto>(role)),
            NotFound);
    }

    [HttpGet("{roleName}/permissions")]
    [ProducesResponseType(typeof(RoleDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> OnGetPermissionsAsync([FromRoute] string roleName)
    {
        var result = await _roleService.GetRoleByName(roleName);

        return result.Match<IActionResult>(
            role => Ok(role.PermissionSet),
            NotFound);
    }

    [HttpGet("{roleName}/users")]
    [ProducesResponseType(typeof(UserDto[]), StatusCodes.Status200OK)]
    public async Task<IActionResult> OnGetUsersAsync(
        [FromRoute] string roleName,
        CancellationToken cancellationToken)
    {
        var result = await _roleService.GetRoleUsersAsync(roleName, cancellationToken);
        
        var dtos = Mapper.Map<UserDto[]>(result);

        return Ok(dtos);
    }

    [HttpPost]
    [ProducesResponseType(typeof(Guid), StatusCodes.Status200OK)]
    public async Task<IActionResult> OnAddAsync([FromBody] RoleAddDto payload)
    {
        var result = await _roleService.AddRoleAsync(
            payload.Name,
            (uint)payload.Priority,
            payload.Color,
            payload.Permissions);

        return result.Match<IActionResult>(
            role => Ok(Mapper.Map<RoleDto>(role)),
            BadRequest);
    }
    
    [HttpPut("{roleName}")]
    [ProducesResponseType(typeof(Guid), StatusCodes.Status200OK)]
    public async Task<IActionResult> OnChangeAsync(
        [FromRoute(Name = "roleName")] string roleNameIdentity,
        [FromBody] RoleUpdateDto payload)
    {
        var result = await _roleService.UpdateAsync(
            roleNameIdentity,
            payload.RoleName,
            (uint)payload.Priority,
            payload.Color,
            payload.PermissionSet
            );

        return result.Match<IActionResult>(
            role => Ok(Mapper.Map<RoleDto>(role)),
            NotFound,
            BadRequest);
    }

    [HttpPost("{roleName}/user/{username}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> OnAddAsync(
        [FromRoute] string roleName,
        [FromRoute] string username,
        CancellationToken cancellationToken)
    {
        var result = await _userService.AddUserToRoleAsync(
            roleName,
            username,
            cancellationToken);

        return result.Match<IActionResult>(
            _ => Ok(),
            NotFound,
            NotFound,
            BadRequest);
    }
    
    [HttpDelete("{roleName}")]
    [ProducesResponseType(typeof(Guid), StatusCodes.Status200OK)]
    public async Task<IActionResult> OnDeleteAsync([FromRoute] string roleName)
    {
        var result = await _roleService.DeleteAsync(roleName);

        return result.Match<IActionResult>(
            _ => Ok(),
            NotFound,
            BadRequest);
    }
}