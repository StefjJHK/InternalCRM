namespace BIP.InternalCRM.WebApi.Analytics.Dtos;

public record ChartSeriesValue<TValue>(
    string Label,
    TValue Value
) where TValue : struct;