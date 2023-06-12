using AutoMapper;
using BIP.InternalCRM.Shopify.Entities;
using BIP.InternalCRM.WebApi.Shopify.Dto;

namespace BIP.InternalCRM.WebApi.Shopify;

public class ShopifyDtoProfile : Profile
{
    public ShopifyDtoProfile()
    {
        CreateMap<ShopifyProductId, ulong>()
            .ConvertUsing(id => id.Value);
        CreateMap<ulong, ShopifyProductId>()
            .ConvertUsing(value => new(value));

        CreateMap<ShopifyProduct, ShopifyProductDto>();
    }
}
