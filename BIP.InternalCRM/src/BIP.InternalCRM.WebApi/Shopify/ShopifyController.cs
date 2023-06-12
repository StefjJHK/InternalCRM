using BIP.InternalCRM.Infrastructure;
using BIP.InternalCRM.Shopify.Commands;
using BIP.InternalCRM.Shopify.Entities;
using BIP.InternalCRM.Shopify.Queries;
using BIP.InternalCRM.WebApi.Products.Dto;
using BIP.InternalCRM.WebApi.Shopify.Dto;
using Microsoft.AspNetCore.Mvc;

namespace BIP.InternalCRM.WebApi.Shopify;

[Route("shopify")]
public class ShopifyController : ApiControllerBase
{
    [HttpGet("products")]
    [ProducesResponseType(typeof(ShopifyProductDto[]), StatusCodes.Status200OK)]
    public async Task<IActionResult> OnGetAllAsync(CancellationToken cancellationToken)
    {
        var query = new ShopifyGetAllProductsQuery();

        var result = await Mediator.Send(query, cancellationToken);

        var dto = Mapper.Map<ShopifyProductDto[]>(result);

        return Ok(dto);
    }

    [HttpGet("products/{shopifyProductId:long}")]
    [ProducesResponseType(typeof(ShopifyProductDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> OnGetAllAsync(
        [FromRoute] ulong shopifyProductId,
        CancellationToken cancellationToken)
    {
        var query = new ShopifyGetProductById(new ShopifyProductId(shopifyProductId));

        var result = await Mediator.Send(query, cancellationToken);

        var dto = Mapper.Map<ShopifyProductDto>(result);

        return Ok(dto);
    }

    [HttpPost("products/{shopifyProductId:long}/export")]
    [ProducesResponseType(typeof(ProductDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> OnExportProductAsync(
        [FromRoute] ulong shopifyProductId,
        CancellationToken cancellationToken)
    {
        var cmd = new ShopifyExportProductCommand(new ShopifyProductId(shopifyProductId));

        var result = await Mediator.Send(cmd, cancellationToken);

        var dto = Mapper.Map<ProductDto>(result);

        return Ok(dto);
    }
}
