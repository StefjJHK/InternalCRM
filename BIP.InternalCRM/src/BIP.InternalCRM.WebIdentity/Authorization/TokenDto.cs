using BIP.InternalCRM.WebIdentity.Permissions;
using BIP.InternalCRM.WebIdentity.Users.Dtos;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace BIP.InternalCRM.WebIdentity.Authorization;

[JsonObject(NamingStrategyType = typeof(SnakeCaseNamingStrategy))]
public record TokenDto(
    string AccessToken,
    long Exp,
    UserTokenDto User,
    PermissionSet Permissions
);