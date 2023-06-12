using AutoMapper;
using BIP.InternalCRM.WebIdentity.Users.Dtos;

namespace BIP.InternalCRM.WebIdentity.Users;

public class UserDtoProfile : Profile
{
    public UserDtoProfile()
    {
        ShouldUseConstructor = t => t.IsPublic;

        CreateMap<User, UserDto>();
        CreateMap<User, UserTokenDto>();
    }
}