namespace BIP.InternalCRM.WebApi.Analytics.Dtos;

public record ChartTotalRevenueReqDto(
    string Quarter,
    int Year,
    string? ProductName = null
);