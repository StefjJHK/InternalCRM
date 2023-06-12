using BIP.InternalCRM.Application;
using BIP.InternalCRM.Application.Customers.Commands;
using BIP.InternalCRM.Application.Customers.Queries;
using BIP.InternalCRM.Infrastructure;
using BIP.InternalCRM.WebApi.Customers.Dto;
using Microsoft.AspNetCore.Mvc;

namespace BIP.InternalCRM.WebApi.Customers;

[Route("customers")]
public class CustomerController : ApiControllerBase
{
    [HttpGet]
    [ProducesResponseType(typeof(CustomerDto[]), StatusCodes.Status200OK)]
    public async Task<IActionResult> OnGetAllAsync(
        [FromQuery(Name = "product")] string? productName,
        CancellationToken cancellationToken = default)
    {
        var query = new GetCustomersQuery(
            PaginationOptions.Empty,
            productName);
        
        var result = await Mediator.Send(query, cancellationToken);

        var dtos = Mapper.Map<CustomerDto[]>(result);

        return Ok(dtos);
    }
    
    [HttpGet("{customerName}")]
    [ProducesResponseType(typeof(CustomerDto[]), StatusCodes.Status200OK)]
    public async Task<IActionResult> OnGetAsync(
        [FromRoute(Name = "customerName")] string customerName,
        CancellationToken cancellationToken = default)
    {
        var query = new GetCustomerQuery(customerName);
        
        var result = await Mediator.Send(query, cancellationToken);

        return result.Match<IActionResult>(
            value => Ok(Mapper.Map<CustomerDto>(value)),
            NotFound);
    }

    [HttpPost]
    [ProducesResponseType(typeof(CustomerDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> OnAddAsync(
        [FromBody] AddCustomerDto payload,
        CancellationToken cancellationToken)
    {
        var cmd = Mapper.Map<AddCustomerCommand>(payload);

        var result = await Mediator.Send(cmd, cancellationToken);

        return result.Match<IActionResult>(
            value => Ok(Mapper.Map<CustomerDto>(value)),
            BadRequest);
    }

    [HttpPost("lead/{leadName}")]
    [ProducesResponseType(typeof(CustomerDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> OnAddFromLeadAsync(
        [FromRoute(Name = "leadName")] string leadName,
        CancellationToken cancellationToken)
    {
        var cmd = new AddCustomerFromLeadCommand(leadName);

        var result = await Mediator.Send(cmd, cancellationToken);

        return result.Match<IActionResult>(
            value => Ok(Mapper.Map<CustomerDto>(value)),
            NotFound,
            BadRequest);
    }
    
    [HttpPut("{customerName}")]
    [ProducesResponseType(typeof(CustomerDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> OnChangeAsync(
        [FromRoute] string customerName,
        [FromBody] ChangeCustomerDto payload,
        CancellationToken cancellationToken)
    {
        var cmd = Mapper.Map<ChangeCustomerCommand>(
            payload,
            o => o.Items.TryAdd(
                nameof(ChangeCustomerCommand.NameIdentity),
                customerName));
        
        var result = await Mediator.Send(cmd, cancellationToken);

        return result.Match<IActionResult>(
            value => Ok(Mapper.Map<CustomerDto>(value)),
            NotFound,
            BadRequest);
    }
    
    [HttpDelete("{customerName}")]
    [ProducesResponseType(typeof(CustomerDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> OnRemoveAsync(
        [FromRoute] string customerName,
        CancellationToken cancellationToken)
    {
        var cmd = new RemoveCustomerCommand(customerName);
        
        var result = await Mediator.Send(cmd, cancellationToken);

        return result.Match<IActionResult>(_ => Ok(), NotFound);
    }
}
