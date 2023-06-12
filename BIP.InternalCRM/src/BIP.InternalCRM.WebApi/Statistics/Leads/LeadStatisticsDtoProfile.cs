using AutoMapper;
using BIP.InternalCRM.Application.Statistics.Leads;

namespace BIP.InternalCRM.WebApi.Statistics.Leads;

public class LeadStatisticsDtoProfile : Profile
{
    public LeadStatisticsDtoProfile()
    {
        CreateMap<LeadSummaryStatistics, LeadSummaryStatisticsDto>();
    }
}