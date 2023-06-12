using AutoMapper;
using BIP.InternalCRM.Application.Contexts;
using BIP.InternalCRM.Domain.DomainErrors;
using BIP.InternalCRM.Domain.Products;
using BIP.InternalCRM.Shopify.AccessService;
using BIP.InternalCRM.Shopify.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using OneOf;

namespace BIP.InternalCRM.Shopify.Commands;

public record ShopifyExportProductCommand(
    ShopifyProductId ShopifyProductId
) : IRequest<OneOf<Product, DomainError>>
{
    public class Handler :
        IRequestHandler<ShopifyExportProductCommand, OneOf<Product, DomainError>>
    {
        private readonly IDomainDbContext _domainDbContext;
        private readonly IShopifyDbContext _shopifyDbContext;
        private readonly IShopifyAccessService _shopifyAccessService;
        private readonly IMapper _mapper;

        public Handler(
            IDomainDbContext domainDbContext,
            IShopifyDbContext shopifyDbContext,
            IShopifyAccessService shopifyAccessService,
            IMapper mapper)
        {
            _domainDbContext = domainDbContext;
            _shopifyDbContext = shopifyDbContext;
            _shopifyAccessService = shopifyAccessService;
            _mapper = mapper;
        }

        public async Task<OneOf<Product, DomainError>> Handle(ShopifyExportProductCommand request,
            CancellationToken cancellationToken)
        {
            var shopifyProductDto =
                await _shopifyAccessService.GetProductAsync(request.ShopifyProductId, cancellationToken);
            var shopifyProduct = _mapper.Map<ShopifyProduct>(shopifyProductDto);

            var otherDomainProductsNames = await _domainDbContext.Products
                .Select(_ => _.Name)
                .ToListAsync(cancellationToken);

            var domainProduct = Product.Create(
                new ProductId(Guid.NewGuid()), shopifyProduct.Name, otherDomainProductsNames);

            if (domainProduct.Value is DomainError domainError) return domainError;

            await _domainDbContext.Products.AddAsync(domainProduct.AsT0, cancellationToken);
            await _shopifyDbContext.ProductsRelations.AddAsync(
                new(shopifyProduct.Id, domainProduct.AsT0.Id),
                cancellationToken);

            return domainProduct.AsT0;
        }
    }
}