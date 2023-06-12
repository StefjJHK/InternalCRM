using System.ComponentModel.DataAnnotations;

namespace BIP.InternalCRM.Shopify;

public record ShopifyOptions
{
    [Required]
    public string AccessToken { get; set; }

    [Required]
    public Uri ShopBaseUrl { get; set; }

    [Required]
    public string ApiVersion { get; set; }
}
