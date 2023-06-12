using System.ComponentModel.DataAnnotations;

namespace BIP.InternalCRM.Application.Subscriptions;

public record SubscriptionNumberOptions
{
    [Required]
    public string Pattern { get; set; } = null!;
}