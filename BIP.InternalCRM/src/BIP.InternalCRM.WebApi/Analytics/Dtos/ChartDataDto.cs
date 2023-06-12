namespace BIP.InternalCRM.WebApi.Analytics.Dtos;

public record ChartDataDto<TValue>(
    ChartSeriesValue<TValue>[] ChartSeries
) where TValue : struct;