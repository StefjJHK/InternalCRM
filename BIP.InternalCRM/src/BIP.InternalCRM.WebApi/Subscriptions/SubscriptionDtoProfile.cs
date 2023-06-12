using AutoMapper;
using BIP.InternalCRM.Application.Extensions;
using BIP.InternalCRM.Application.Subscriptions.Commands;
using BIP.InternalCRM.Application.Subscriptions.QueryResults;
using BIP.InternalCRM.Domain.Subscriptions;
using BIP.InternalCRM.WebApi.Subscriptions.Dto;

namespace BIP.InternalCRM.WebApi.Subscriptions;

public class SubscriptionDtoProfile : Profile
{
    public SubscriptionDtoProfile()
    {
        ShouldUseConstructor = (t) => t.IsPublic;

        CreateMap<AddSubscriptionDto, AddSubscriptionCommand>()
            .ForRecordParameter(
                _ => _.InvoiceNumber,
                o => o.MapFromItems<AddSubscriptionDto, string>(nameof(AddSubscriptionCommand.InvoiceNumber)));

        CreateMap<Subscription, SubscriptionDto>()
            .ForRecordParameter(_ => _.IsPaid, o => o.MapFrom<bool?>(_ => null));
        CreateMap<SubscriptionQueryResult, SubscriptionDto>()
            .IncludeMembers(_ => _.Subscription)
            .ForRecordParameter(
                _ => _.IsPaid,
                o => o.MapFrom(src => src.Subscription.PaidDate.HasValue));
    }
}