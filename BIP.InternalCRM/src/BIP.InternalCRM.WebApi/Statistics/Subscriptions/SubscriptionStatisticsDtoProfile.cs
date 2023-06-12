using AutoMapper;
using BIP.InternalCRM.Application.Statistics.Subscriptions;

namespace BIP.InternalCRM.WebApi.Statistics.Subscriptions;

public class SubscriptionStatisticsDtoProfile : Profile
{
    public SubscriptionStatisticsDtoProfile()
    {
        CreateMap<SubscriptionSummaryStatistics, SubscriptionSummaryStatisticsDto>();
    }
}