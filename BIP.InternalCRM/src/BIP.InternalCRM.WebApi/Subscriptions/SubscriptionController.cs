using BIP.InternalCRM.Application;
using BIP.InternalCRM.Application.Subscriptions.Queries;
using BIP.InternalCRM.Infrastructure;
using BIP.InternalCRM.WebApi.Subscriptions.Dto;
using Microsoft.AspNetCore.Mvc;

namespace BIP.InternalCRM.WebApi.Subscriptions;

[Route("subscriptions")]
public class SubscriptionController : ApiControllerBase
{
    [HttpGet]
    [ProducesResponseType(typeof(SubscriptionDto[]), StatusCodes.Status200OK)]
    public async Task<IActionResult> OnGetAllAsync(
        [FromQuery(Name = "product")] string? productName,
        [FromQuery(Name = "customer")] string? customerName,
        [FromQuery(Name = "invoice")] string? invoiceNumber,
        CancellationToken cancellationToken)
    {
        var query = new GetSubscriptionsQuery(
            PaginationOptions.Empty,
            productName,
            customerName,
            invoiceNumber);
        
        var result = await Mediator.Send(query, cancellationToken);
        
        var dtos = Mapper.Map<SubscriptionDto[]>(result);

        return Ok(dtos);
    }
    
    [HttpGet("{subscriptionNumber}")]
    [ProducesResponseType(typeof(SubscriptionDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> OnGetAllAsync(
        [FromRoute(Name = "subscriptionNumber")] string subscriptionNumber,
        CancellationToken cancellationToken)
    {
        var query = new GetSubscriptionQuery(subscriptionNumber);
        
        var result = await Mediator.Send(query, cancellationToken);

        return result.Match<IActionResult>(
            value => Ok(Mapper.Map<SubscriptionDto[]>(value)),
            NotFound);
    }

    [HttpGet("licenses/{licKey:guid}")]
    public async Task<IActionResult> OnGetLicenseAsync(
        [FromRoute] Guid licKey,
        CancellationToken cancellationToken)
    {
        var query = new GetIlLicenseQuery(licKey);

        var result = await Mediator.Send(query, cancellationToken);

        return result.Match<IActionResult>(
            value => File(
                fileContents: value.Data,
                contentType: "application/octet-stream",
                fileDownloadName: value.Filename),
            NotFound);
    }
}