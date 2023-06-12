using AutoMapper;
using BIP.InternalCRM.Application.Analytics.TotalCustomers;
using BIP.InternalCRM.Application.Analytics.TotalRevenue;
using BIP.InternalCRM.Application.Analytics.TotalSales;
using BIP.InternalCRM.Application.Extensions;
using BIP.InternalCRM.WebApi.Analytics.Dtos;

namespace BIP.InternalCRM.WebApi.Analytics;

public class AnalyticsDtoProfile : Profile
{
    public AnalyticsDtoProfile()
    {
        CreateMap<ChartTotalCustomersReqDto, GetTotalCustomersDataQuery>();

        CreateMap<GetTotalCustomersDataQuery.Model, ChartSeriesValue<int>>()
            .ForRecordParameter(_ => _.Label, o => o.MapFrom(src => src.Month))
            .ForRecordParameter(_ => _.Value, o => o.MapFrom(src => src.TotalCustomers));
        CreateMap<GetTotalCustomersDataQuery.Result, ChartTotalCustomersResDto>()
            .ForRecordParameter(
                _ => _.Customers,
                o => o.MapFrom<ChartDataDto<int>>((src, ctx) =>
                    new(ctx.Mapper.Map<ChartSeriesValue<int>[]>(src.Customers))));

        CreateMap<ChartTotalSalesReqDto, GetTotalSalesDataQuery>();
        
        CreateMap<GetTotalSalesDataQuery.Model, ChartSeriesValue<decimal>>()
            .ForRecordParameter(_ => _.Label, o => o.MapFrom(src => src.Year))
            .ForRecordParameter(_ => _.Value, o => o.MapFrom(src => src.TotalSales));
        CreateMap<GetTotalSalesDataQuery.Result, ChartTotalSalesResDto>()
            .ForRecordParameter(
                _ => _.Sales,
                o => o.MapFrom<ChartDataDto<decimal>>((src, ctx) =>
                    new(ctx.Mapper.Map<ChartSeriesValue<decimal>[]>(src.Sales))));

        CreateMap<ChartTotalRevenueReqDto, GetTotalRevenueDataQuery>();
        
        CreateMap<GetTotalRevenueDataQuery.Model, ChartSeriesValue<decimal>>()
            .ForRecordParameter(_ => _.Label, o => o.MapFrom(src => src.Week))
            .ForRecordParameter(_ => _.Value, o => o.MapFrom(src => src.Amount));
        CreateMap<GetTotalRevenueDataQuery.Result, ChartTotalRevenueResDto>()
            .ForRecordParameter(
                _ => _.Revenue,
                o => o.MapFrom<ChartDataDto<decimal>>((src, ctx) =>
                    new(ctx.Mapper.Map<ChartSeriesValue<decimal>[]>(src.Revenue))));
    }
}