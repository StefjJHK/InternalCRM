namespace BIP.InternalCRM.WebIdentity.Users.Dtos;

public record UserChangePasswordDto(
    string OldPassword,
    string NewPassword
);