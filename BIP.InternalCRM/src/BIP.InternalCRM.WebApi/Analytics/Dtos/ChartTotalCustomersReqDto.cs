namespace BIP.InternalCRM.WebApi.Analytics.Dtos;

public record ChartTotalCustomersReqDto(
    DateTime StartDate,
    DateTime EndDate
);