using AutoMapper;
using BIP.InternalCRM.Shopify.AccessService.Dto;
using BIP.InternalCRM.Shopify.Entities;

namespace BIP.InternalCRM.Shopify.AccessService;

public class ShopifyAccessProfile : Profile
{
    public ShopifyAccessProfile()
    {
        CreateMap<ShopifyProductAccessDto, ShopifyProduct>()
            .ForMember(
                _ => _.Id,
                o => o.MapFrom(src => new ShopifyProductId(src.ShopifyId)));
    }
}
