using BIP.InternalCRM.Application.Analytics.TotalCustomers;
using BIP.InternalCRM.Application.Analytics.TotalRevenue;
using BIP.InternalCRM.Application.Analytics.TotalSales;
using BIP.InternalCRM.Infrastructure;
using BIP.InternalCRM.WebApi.Analytics.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace BIP.InternalCRM.WebApi.Analytics;

[Route("analytics")]
public class AnalyticsController : ApiControllerBase
{
    [HttpGet("total-revenue")]
    [ProducesResponseType(typeof(ChartTotalRevenueResDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> OnGetChartRevenueChartAsync(
        [FromQuery] ChartTotalRevenueReqDto request,
        CancellationToken cancellationToken)
    {
        var query = Mapper.Map<GetTotalRevenueDataQuery>(request);

        var result = await Mediator.Send(query, cancellationToken);

        var dto = Mapper.Map<ChartTotalRevenueResDto>(result);

        return Ok(dto);
    }
    
    [HttpGet("total-customers")]
    [ProducesResponseType(typeof(ChartTotalCustomersResDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> OnGetChartTotalCustomersAsync(
        [FromQuery] ChartTotalCustomersReqDto request,
        CancellationToken cancellationToken)
    {
        var query = Mapper.Map<GetTotalCustomersDataQuery>(request);

        var result = await Mediator.Send(query, cancellationToken);

        var dto = Mapper.Map<ChartTotalCustomersResDto>(result);

        return Ok(dto);
    }
    
    [HttpGet("total-sales")]
    [ProducesResponseType(typeof(ChartTotalSalesResDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> OnGetChartTotalSalesAsync(
        [FromQuery] ChartTotalSalesReqDto request,
        CancellationToken cancellationToken)
    {
        var query = Mapper.Map<GetTotalSalesDataQuery>(request);

        var result = await Mediator.Send(query, cancellationToken);

        var dto = Mapper.Map<ChartTotalSalesResDto>(result);

        return Ok(dto);
    }
}
