using AutoMapper;
using BIP.InternalCRM.Application.Extensions;
using BIP.InternalCRM.WebIdentity.Roles.Dtos;
using BIP.InternalCRM.WebIdentity.Users.Dtos;

namespace BIP.InternalCRM.WebIdentity.Roles;

public class RoleDtoProfile : Profile
{
    public RoleDtoProfile()
    {
        CreateMap<Role, RoleDto>();
    }
}
