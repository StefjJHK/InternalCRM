using BIP.InternalCRM.Application;
using BIP.InternalCRM.Application.Leads.Commands;
using BIP.InternalCRM.Application.Leads.Queries;
using BIP.InternalCRM.Infrastructure;
using BIP.InternalCRM.WebApi.Leads.Dto;
using Microsoft.AspNetCore.Mvc;

namespace BIP.InternalCRM.WebApi.Leads;

[Route("leads")]
public class LeadController : ApiControllerBase
{
    [HttpGet]
    [ProducesResponseType(typeof(LeadDto[]), StatusCodes.Status200OK)]
    public async Task<IActionResult> OnGetAllAsync(
        [FromQuery(Name = "product")] string? productName, 
        CancellationToken cancellationToken)
    {
        var query = new GetLeadsQuery(
            PaginationOptions.Empty,
            productName);
        
        var result = await Mediator.Send(query, cancellationToken);
        
        var dtos = Mapper.Map<LeadDto[]>(result);

        return Ok(dtos);
    }
    
    [HttpGet("{leadName}")]
    [ProducesResponseType(typeof(LeadDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> OnGetAsync(
        [FromRoute(Name = "leadName")] string leadName, 
        CancellationToken cancellationToken)
    {
        var query = new GetLeadQuery(leadName);
        
        var result = await Mediator.Send(query, cancellationToken);
        
        return result.Match<IActionResult>(
            value => Ok(Mapper.Map<LeadDto>(value)),
            NotFound);
    }

    [HttpPost]
    [ProducesResponseType(typeof(LeadDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> OnAddAsync(
        [FromBody] AddLeadDto payload,
        CancellationToken cancellationToken)
    {
        var cmd = Mapper.Map<AddLeadCommand>(payload);

        var result = await Mediator.Send(cmd, cancellationToken);

        return result.Match<IActionResult>(
            value => Ok(Mapper.Map<LeadDto>(value)),
            NotFound,
            BadRequest);
    }
    
    [HttpPut("{leadName}")]
    [ProducesResponseType(typeof(LeadDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> OnChangeAsync(
        [FromRoute(Name = "leadName")] string leadName, 
        [FromBody] ChangeLeadDto payload,
        CancellationToken cancellationToken)
    {
        var cmd = Mapper.Map<ChangeLeadCommand>(
            payload,
            o => o.Items.TryAdd(
                nameof(ChangeLeadCommand.NameIdentity),
                leadName));

        var result = await Mediator.Send(cmd, cancellationToken);
        
        return result.Match<IActionResult>(
            value => Ok(Mapper.Map<LeadDto>(value)),
            NotFound,
            BadRequest);
    }
    
    [HttpDelete("{leadName}")]
    [ProducesResponseType(typeof(LeadDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> OnChangeAsync(
        [FromRoute(Name = "leadName")] string leadName, 
        CancellationToken cancellationToken)
    {
        var cmd = new RemoveLeadCommand(leadName);

        var result = await Mediator.Send(cmd, cancellationToken);
        
        return result.Match<IActionResult>(_ => Ok(), NotFound);
    }
}