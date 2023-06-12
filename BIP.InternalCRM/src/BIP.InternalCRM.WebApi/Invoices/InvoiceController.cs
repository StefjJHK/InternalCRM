using BIP.InternalCRM.Application;
using BIP.InternalCRM.Application.Invoices.Commands;
using BIP.InternalCRM.Application.Invoices.Queries;
using BIP.InternalCRM.Application.Payments.Commands;
using BIP.InternalCRM.Application.Payments.Queries;
using BIP.InternalCRM.Application.Subscriptions.Commands;
using BIP.InternalCRM.Infrastructure;
using BIP.InternalCRM.WebApi.Invoices.Dto;
using BIP.InternalCRM.WebApi.Payments.Dtos;
using BIP.InternalCRM.WebApi.Subscriptions.Dto;
using Microsoft.AspNetCore.Mvc;

namespace BIP.InternalCRM.WebApi.Invoices;

[Route("invoices")]
public class InvoiceController : ApiControllerBase
{
    [HttpGet]
    [ProducesResponseType(typeof(InvoiceDto[]), StatusCodes.Status200OK)]
    public async Task<IActionResult> OnGetAllAsync(
        [FromQuery(Name = "product")] string? productName,
        [FromQuery(Name = "customer")] string? customerName,
        [FromQuery(Name = "purchaseOrder")] string? purchaseOrderNumber,
        CancellationToken cancellationToken)
    {
        var query = new GetInvoicesQuery(PaginationOptions.Empty,
            productName,
            customerName,
            purchaseOrderNumber);

        var result = await Mediator.Send(query, cancellationToken);
        
        var dtos = Mapper.Map<InvoiceDto[]>(result);

        return Ok(dtos);
    }

    [HttpGet("{invoiceNumber}")]
    [ProducesResponseType(typeof(InvoiceDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> OnGetAsync(
        [FromRoute(Name = "invoiceNumber")] string invoiceNumber,
        CancellationToken cancellationToken)
    {
        var query = new GetInvoiceQuery(invoiceNumber);

        var result = await Mediator.Send(query, cancellationToken);

        return result.Match<IActionResult>(
            value => Ok(Mapper.Map<InvoiceDto>(value)),
            NotFound);
    }

    [HttpPost]
    [ProducesResponseType(typeof(InvoiceDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> OnAddAsync(
        [FromBody] AddInvoiceDto payload,
        CancellationToken cancellationToken)
    {
        var cmd = Mapper.Map<AddInvoiceCommand>(payload);

        var result = await Mediator.Send(cmd, cancellationToken);

        return result.Match<IActionResult>(
            value => Ok(Mapper.Map<InvoiceDto>(value)),
            NotFound,
            BadRequest);
    }

    [HttpPut("{invoiceNumber}")]
    [ProducesResponseType(typeof(InvoiceDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> OnChangeAsync(
        [FromRoute] string invoiceNumber,
        [FromBody] ChangeInvoiceDto payload,
        CancellationToken cancellationToken)
    {
        var cmd = Mapper.Map<ChangeInvoiceCommand>(
            payload,
            o => o.Items.TryAdd(
                nameof(ChangeInvoiceCommand.NumberIdentity),
                invoiceNumber));

        var result = await Mediator.Send(cmd, cancellationToken);

        return result.Match<IActionResult>(
            value => Ok(Mapper.Map<InvoiceDto>(value)),
            NotFound,
            BadRequest);
    }

    [HttpDelete("{invoiceNumber}")]
    [ProducesResponseType(typeof(InvoiceDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> OnRemoveAsync(
        [FromRoute] string invoiceNumber,
        CancellationToken cancellationToken)
    {
        var cmd = new RemoveInvoiceCommand(invoiceNumber);

        var result = await Mediator.Send(cmd, cancellationToken);

        return result.Match<IActionResult>(_ => Ok(), NotFound);
    }

    [HttpPost("{invoiceNumber}/payments")]
    [ProducesResponseType(typeof(PaymentDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> OnAddPaymentAsync(
        [FromRoute(Name = "invoiceNumber")] string invoiceNumber,
        [FromBody] AddPaymentDto payload,
        CancellationToken cancellationToken)
    {
        var cmd = Mapper.Map<AddPaymentCommand>(
            payload,
            o => o.Items.TryAdd(
                nameof(AddPaymentCommand.InvoiceNumber),
                invoiceNumber));

        var result = await Mediator.Send(cmd, cancellationToken);

        return result.Match<IActionResult>(
            value => Ok(Mapper.Map<PaymentDto>(value)),
            NotFound,
            BadRequest);
    }

    [HttpGet("{invoiceNumber}/payments/{paymentNumber}")]
    [ProducesResponseType(typeof(PaymentDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> OnGetPaymentAsync(
        [FromRoute(Name = "invoiceNumber")] string invoiceNumber,
        [FromRoute(Name = "paymentNumber")] string paymentNumber,
        CancellationToken cancellationToken)
    {
        var query = new GetPaymentQuery(
            invoiceNumber,
            paymentNumber);

        var result = await Mediator.Send(query, cancellationToken);

        return result.Match<IActionResult>(
            value => Ok(Mapper.Map<PaymentDto>(value)),
            NotFound);
    }

    [HttpPut("{invoiceNumber}/payments/{paymentNumber}")]
    [ProducesResponseType(typeof(PaymentDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> OnChangePaymentAsync(
        [FromRoute(Name = "invoiceNumber")] string invoiceNumber,
        [FromRoute(Name = "paymentNumber")] string paymentNumber,
        [FromBody] ChangePaymentDto payload,
        CancellationToken cancellationToken)
    {
        var cmd = Mapper.Map<ChangePaymentCommand>(
            payload,
            o =>
            {
                o.Items.TryAdd(
                    nameof(ChangePaymentCommand.InvoiceNumber),
                    invoiceNumber);

                o.Items.TryAdd(
                    nameof(ChangePaymentCommand.PaymentNumberIdentity),
                    paymentNumber);
            });

        var result = await Mediator.Send(cmd, cancellationToken);

        return result.Match<IActionResult>(
            value => Ok(Mapper.Map<PaymentDto>(value)),
            NotFound,
            BadRequest);
    }

    [HttpDelete("{invoiceNumber}/payments/{paymentNumber}")]
    [ProducesResponseType(typeof(PaymentDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> OnRemovePaymentAsync(
        [FromRoute(Name = "invoiceNumber")] string invoiceNumber,
        [FromRoute(Name = "paymentNumber")] string paymentNumber,
        CancellationToken cancellationToken)
    {
        var query = new RemovePaymentCommand(
            invoiceNumber,
            paymentNumber);

        var result = await Mediator.Send(query, cancellationToken);

        return result.Match<IActionResult>(_ => Ok(), NotFound);
    }

    [HttpPost("{invoiceNumber}/subscriptions")]
    [ProducesResponseType(typeof(SubscriptionDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> OnAddSubscriptionAsync(
        [FromRoute(Name = "invoiceNumber")] string invoiceNumber,
        [FromBody] AddSubscriptionDto payload,
        CancellationToken cancellationToken)
    {
        var cmd = Mapper.Map<AddSubscriptionCommand>(
            payload,
            o => o.Items.TryAdd(
                nameof(AddSubscriptionCommand.InvoiceNumber),
                invoiceNumber));

        var result = await Mediator.Send(cmd, cancellationToken);

        return result.Match<IActionResult>(
            value => Ok(Mapper.Map<SubscriptionDto>(value)),
            NotFound,
            BadRequest);
    }
}