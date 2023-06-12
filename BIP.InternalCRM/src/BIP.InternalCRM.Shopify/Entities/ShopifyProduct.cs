namespace BIP.InternalCRM.Shopify.Entities;

public class ShopifyProduct
{
    public ShopifyProductId Id { get; private init; }

    public string Name { get; private set; }

    public string Status { get; private set; }
}
