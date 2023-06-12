using System.ComponentModel.DataAnnotations;

namespace BIP.InternalCRM.WebIdentity;

public record JwtTokenOptions
{
    [Required] public string Issuer { get; set; } = null!;

    [Required] public string[] Audience { get; set; } = null!;

    [Required] public string Secret { get; set; } = null!;

    [Required] public long ExpiresIn { get; set; } = default!;
}