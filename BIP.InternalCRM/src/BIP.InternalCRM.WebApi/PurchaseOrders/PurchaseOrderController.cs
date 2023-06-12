using BIP.InternalCRM.Application;
using BIP.InternalCRM.Application.PurchaseOrders.Commands;
using BIP.InternalCRM.Application.PurchaseOrders.Queries;
using BIP.InternalCRM.Infrastructure;
using BIP.InternalCRM.WebApi.PurchaseOrders.Dto;
using Microsoft.AspNetCore.Mvc;

namespace BIP.InternalCRM.WebApi.PurchaseOrders;

[Route("purchase-orders")]
public class PurchaseOrderController : ApiControllerBase
{
    [HttpGet]
    [ProducesResponseType(typeof(PurchaseOrderDto[]), StatusCodes.Status200OK)]
    public async Task<IActionResult> OnGetAllAsync(
        [FromQuery(Name = "invoice")] string? invoiceNumber,
        [FromQuery(Name = "product")] string? productName,
        [FromQuery(Name = "customer")] string? customerName,
        CancellationToken cancellationToken)
    {
        var query = new GetPurchaseOrdersQuery(
            PaginationOptions.Empty,
            invoiceNumber,
            productName,
            customerName);
        
        var result = await Mediator.Send(query, cancellationToken);

        var dtos = Mapper.Map<PurchaseOrderDto[]>(result);

        return Ok(dtos);
    }
    
    [HttpGet("{purchaseOrderNumber}")]
    [ProducesResponseType(typeof(PurchaseOrderDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> OnGetAsync(
        [FromRoute(Name = "purchaseOrderNumber")] string poNumber,
        CancellationToken cancellationToken)
    {
        var query = new GetPurchaseOrderQuery(poNumber);
        
        var result = await Mediator.Send(query, cancellationToken);

        return result.Match<IActionResult>(
            value => Ok(Mapper.Map<PurchaseOrderDto>(value)),
            NotFound);
    }

    [HttpPost]
    [ProducesResponseType(typeof(PurchaseOrderDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> OnAddAsync(
        [FromBody] AddPurchaseOrderDto payload,
        CancellationToken cancellationToken)
    {
        var cmd = Mapper.Map<AddPurchaseOrderCommand>(payload);

        var result = await Mediator.Send(cmd, cancellationToken);

        return result.Match<IActionResult>(
            value => Ok(Mapper.Map<PurchaseOrderDto>(value)),
            NotFound,
            BadRequest);
    }
}
