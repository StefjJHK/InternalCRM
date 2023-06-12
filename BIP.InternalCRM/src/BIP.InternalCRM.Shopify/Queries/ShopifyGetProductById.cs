using AutoMapper;
using BIP.InternalCRM.Shopify.AccessService;
using BIP.InternalCRM.Shopify.Entities;
using MediatR;

namespace BIP.InternalCRM.Shopify.Queries;

public record ShopifyGetProductById(
    ShopifyProductId Id
) : IRequest<ShopifyProduct>
{
    public class Handler :
        IRequestHandler<ShopifyGetProductById, ShopifyProduct>
    {
        private readonly IShopifyAccessService _accessService;
        private readonly IMapper _mapper;

        public Handler(IShopifyAccessService accessService, IMapper mapper)
        {
            _accessService = accessService;
            _mapper = mapper;
        }

        public async Task<ShopifyProduct> Handle(ShopifyGetProductById request, CancellationToken cancellationToken)
        {
            var dto = await _accessService.GetProductAsync(request.Id, cancellationToken);

            var product = _mapper.Map<ShopifyProduct>(dto);

            return product;
        }
    }
}
