using BIP.InternalCRM.Application;
using BIP.InternalCRM.Application.Products.Commands;
using BIP.InternalCRM.Application.Products.Queries;
using BIP.InternalCRM.Application.Statistics.Products.Queries;
using BIP.InternalCRM.Infrastructure;
using BIP.InternalCRM.WebApi.Products.Dto;
using Microsoft.AspNetCore.Mvc;

namespace BIP.InternalCRM.WebApi.Products;

[Route("products")]
public class ProductController : ApiControllerBase
{
    [HttpGet("{productName}/customers")]
    [ProducesResponseType(typeof(ProductCustomersTableDto[]), StatusCodes.Status200OK)]
    public async Task<IActionResult> OnGetCustomers(
        [FromRoute] string productName,
        CancellationToken cancellationToken)
    {
        var query = new GetProductCustomersStatisticsQuery(
            PaginationOptions.Empty,
            productName);

        var result = await Mediator.Send(query, cancellationToken);

        var dtos = Mapper.Map<ProductCustomersTableRowDto[]>(result);

        return Ok(dtos);
    }

    [HttpGet]
    [ProducesResponseType(typeof(ProductDto[]), StatusCodes.Status200OK)]
    public async Task<IActionResult> OnGetAllAsync(
        [FromQuery(Name = "customer")] string? customerName,
        CancellationToken cancellationToken = default)
    {
        var query = new GetProductsQuery(
            PaginationOptions.Empty,
            customerName);

        var result = await Mediator.Send(query, cancellationToken);
        
        var dtos = Mapper.Map<ProductDto[]>(result);

        return Ok(dtos);
    }

    [HttpGet("{productName}")]
    [ProducesResponseType(typeof(ProductDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> OnGetAsync(
        [FromRoute(Name = "productName")] string productName,
        CancellationToken cancellationToken)
    {
        var query = new GetProductQuery(productName);

        var result = await Mediator.Send(query, cancellationToken);
        
        return result.Match<IActionResult>(
            value => Ok(Mapper.Map<ProductDto>(value)),
            NotFound);
    }

    [HttpPost]
    [Consumes("multipart/form-data", IsOptional = false)]
    [ProducesResponseType(typeof(ProductDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> OnAddAsync(
        [FromForm] AddProductDto payload,
        CancellationToken cancellationToken)
    {
        var cmd = Mapper.Map<AddProductCommand>(payload);

        var result = await Mediator.Send(cmd, cancellationToken);

        return result.Match<IActionResult>(
            value => Ok(Mapper.Map<ProductDto>(value)),
            BadRequest);
    }

    [HttpPut("{productName}")]
    [Consumes("multipart/form-data", IsOptional = false)]
    [ProducesResponseType(typeof(ProductDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> OnChangeAsync(
        [FromRoute] string productName,
        [FromForm] ChangeProductDto payload,
        CancellationToken cancellationToken)
    {
        var cmd = Mapper.Map<ChangeProductCommand>(payload,
            o => o.Items.TryAdd(
                nameof(ChangeProductCommand.NameIdentity),
                productName));

        var result = await Mediator.Send(cmd, cancellationToken);

        return result.Match<IActionResult>(
            value => Ok(Mapper.Map<ProductDto>(value)),
            NotFound,
            BadRequest);
    }

    [HttpDelete("{productName}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> OnRemoveAsync(
        [FromRoute] string productName,
        CancellationToken cancellationToken)
    {
        var cmd = new RemoveProductCommand(productName);

        var result = await Mediator.Send(cmd, cancellationToken);

        return result.Match<IActionResult>(_ => Ok(), NotFound);
    }
}