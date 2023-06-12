namespace BIP.InternalCRM.WebApi.Analytics.Dtos;

public record ChartTotalCustomersResDto(
    int Total,
    ChartDataDto<int> Customers
);