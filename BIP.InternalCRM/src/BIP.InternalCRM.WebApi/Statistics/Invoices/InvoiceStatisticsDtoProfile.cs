using AutoMapper;
using BIP.InternalCRM.Application.Extensions;
using BIP.InternalCRM.Application.Statistics.Invoices;

namespace BIP.InternalCRM.WebApi.Statistics.Invoices;

public class InvoiceStatisticsDtoProfile : Profile
{
    public InvoiceStatisticsDtoProfile()
    {
        CreateMap<InvoiceSummaryStatistics, InvoiceSummaryStatisticsDto>()
            .ForRecordParameter(
                _ => _.PercentageOfPaid,
                o => o.MapFrom(src => 1f * src.NumberOfPaid / src.TotalInvoices))
            .ForRecordParameter(
                _ => _.PercentageOfOverdue,
                o => o.MapFrom(src => 1f * src.NumberOfOverdue / src.TotalInvoices));
    }
}