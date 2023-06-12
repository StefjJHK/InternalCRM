using AutoMapper;
using BIP.InternalCRM.Application.Statistics.Payments;

namespace BIP.InternalCRM.WebApi.Statistics.Payments;

public class PaymentStatisticsDtoProfile : Profile
{
    public PaymentStatisticsDtoProfile()
    {
        CreateMap<PaymentSummaryStatistics, PaymentSummaryStatisticsDto>();
    }
}