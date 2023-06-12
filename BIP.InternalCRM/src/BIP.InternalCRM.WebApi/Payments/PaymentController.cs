using BIP.InternalCRM.Application;
using BIP.InternalCRM.Application.Payments.Queries;
using BIP.InternalCRM.Infrastructure;
using BIP.InternalCRM.WebApi.Payments.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace BIP.InternalCRM.WebApi.Payments;

[Route("payments")]
public class PaymentController : ApiControllerBase
{
    [HttpGet]
    [ProducesResponseType(typeof(PaymentDto[]), StatusCodes.Status200OK)]
    public async Task<IActionResult> OnGetAllAsync(
        [FromQuery(Name = "purchaseOrder")] string? purchaseOrderNumber,
        [FromQuery(Name = "invoice")] string? invoiceNumber,
        [FromQuery(Name = "subscription")] string? subscriptionNumber,
        [FromQuery(Name = "product")] string? productName,
        [FromQuery(Name = "customer")] string? customerName,
        CancellationToken cancellationToken)
    {
        var query = new GetPaymentsQuery(
            PaginationOptions.Empty,
            purchaseOrderNumber,
            invoiceNumber,
            subscriptionNumber,
            productName,
            customerName);

        var result = await Mediator.Send(query, cancellationToken);
        
        var dtos = Mapper.Map<PaymentDto[]>(result);

        return Ok(dtos);
    }
}