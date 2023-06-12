using System.ComponentModel.DataAnnotations;

namespace BIP.InternalCRM.Application.FileStorage;

public record FileStorageOptions
{
    [Required]
    public string Path { get; set; } = null!;
}
