namespace BIP.InternalCRM.WebApi.Analytics.Dtos;

public record ChartTotalSalesResDto(
    decimal Total,
    ChartDataDto<decimal> Sales 
);