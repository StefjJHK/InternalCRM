using AutoMapper;
using BIP.InternalCRM.Shopify.AccessService;
using BIP.InternalCRM.Shopify.Entities;
using MediatR;

namespace BIP.InternalCRM.Shopify.Queries;

public record ShopifyGetAllProductsQuery :
    IRequest<IReadOnlyCollection<ShopifyProduct>>
{
    public class Handler :
        IRequestHandler<ShopifyGetAllProductsQuery, IReadOnlyCollection<ShopifyProduct>>
    {
        private readonly IShopifyAccessService _accessService;
        private readonly IMapper _mapper;

        public Handler(IShopifyAccessService accessService, IMapper mapper)
        {
            _accessService = accessService;
            _mapper = mapper;
        }

        public async Task<IReadOnlyCollection<ShopifyProduct>> Handle(ShopifyGetAllProductsQuery request, CancellationToken cancellationToken)
        {
            var dtos = await _accessService.GetProductsAsync(cancellationToken);

            var products = _mapper.Map<ShopifyProduct[]>(dtos);

            return products;
        }
    }
}
