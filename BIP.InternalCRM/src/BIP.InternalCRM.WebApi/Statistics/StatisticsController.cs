using BIP.InternalCRM.Application;
using BIP.InternalCRM.Application.Statistics.Invoices;
using BIP.InternalCRM.Application.Statistics.Leads;
using BIP.InternalCRM.Application.Statistics.Payments;
using BIP.InternalCRM.Application.Statistics.Products.Queries;
using BIP.InternalCRM.Application.Statistics.Subscriptions;
using BIP.InternalCRM.Infrastructure;
using BIP.InternalCRM.WebApi.Statistics.Invoices;
using BIP.InternalCRM.WebApi.Statistics.Leads;
using BIP.InternalCRM.WebApi.Statistics.Payments;
using BIP.InternalCRM.WebApi.Statistics.Products;
using BIP.InternalCRM.WebApi.Statistics.Subscriptions;
using Microsoft.AspNetCore.Mvc;

namespace BIP.InternalCRM.WebApi.Statistics;

[Route("statistics")]
public class StatisticsController : ApiControllerBase
{
    [HttpGet("products")]
    [HttpGet("products/{productName?}")]
    [ProducesResponseType(typeof(ProductStatisticsDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> OnGetProductStatisticsAsync(
        [FromRoute] string? productName = null,
        CancellationToken cancellationToken = default)
    {
        var query = new GetProductStatisticsQuery(PaginationOptions.Empty, productName);

        var result = await Mediator.Send(query, cancellationToken);

        return result.Match<IActionResult>(
            value => Ok(Mapper.Map<ProductStatisticsDto>(value)),
            NotFound,
            values => Ok(Mapper.Map<ProductStatisticsDto[]>(values)));
    }
    
    [HttpGet("leads/summary")]
    [ProducesResponseType(typeof(LeadSummaryStatisticsDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> OnGetLeadSummaryStatisticsAsync(CancellationToken cancellationToken)
    {
        var query = new GetLeadSummaryStatisticsQuery();

        var result = await Mediator.Send(query, cancellationToken);

        var dto = Mapper.Map<LeadSummaryStatisticsDto>(result);

        return Ok(dto);
    }

    [HttpGet("invoices/summary")]
    [ProducesResponseType(typeof(InvoiceSummaryStatisticsDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> OnGetInvoiceSummaryStatisticsAsync(CancellationToken cancellationToken)
    {
        var query = new GetInvoiceSummaryStatisticsQuery();

        var result = await Mediator.Send(query, cancellationToken);

        var dto = Mapper.Map<InvoiceSummaryStatisticsDto>(result);

        return Ok(dto);
    }
    
    [HttpGet("subscriptions/summary")]
    [ProducesResponseType(typeof(SubscriptionSummaryStatisticsDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> OnGetSubscriptionSummaryStatisticsAsync(CancellationToken cancellationToken)
    {
        var query = new GetSubscriptionSummaryStatisticsQuery();

        var result = await Mediator.Send(query, cancellationToken);

        var dto = Mapper.Map<SubscriptionSummaryStatisticsDto>(result);

        return Ok(dto);
    }
    
    [HttpGet("payments/summary")]
    [ProducesResponseType(typeof(PaymentSummaryStatisticsDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> OnGetPaymentSummaryStatisticsAsync(CancellationToken cancellationToken)
    {
        var query = new GetPaymentSummaryStatisticsQuery();

        var result = await Mediator.Send(query, cancellationToken);

        var dto = Mapper.Map<PaymentSummaryStatisticsDto>(result);

        return Ok(dto);
    }
}