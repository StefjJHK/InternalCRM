using System.IdentityModel.Tokens.Jwt;
using AutoMapper;
using BIP.InternalCRM.Application.Extensions;

namespace BIP.InternalCRM.WebIdentity.Authorization;

public class AuthDtoProfile : Profile
{
    public AuthDtoProfile()
    {
        ShouldUseConstructor = t => t.IsPublic;
        
        CreateMap<JwtSecurityToken, TokenDto>()
            .ForRecordParameter(
                _ => _.AccessToken,
                o => o.MapFrom(src => new JwtSecurityTokenHandler().WriteToken(src)))
            .ForRecordParameter(
                _ => _.Exp,
                o => o.MapFrom(src => src.Payload.Exp))
            .ForRecordParameter(
                _ => _.User,
                o => o.MapFrom(src => src.Payload["user"]))
            .ForRecordParameter(
                _ => _.Permissions,
                o => o.MapFrom(src => src.Payload["permissions"]));
    }
}