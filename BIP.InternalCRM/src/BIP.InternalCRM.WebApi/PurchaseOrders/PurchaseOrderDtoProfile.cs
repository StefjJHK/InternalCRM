using AutoMapper;
using BIP.InternalCRM.Application.Extensions;
using BIP.InternalCRM.Application.PurchaseOrders.Commands;
using BIP.InternalCRM.Application.PurchaseOrders.QueryResults;
using BIP.InternalCRM.Domain.PurchaseOrders;
using BIP.InternalCRM.WebApi.PurchaseOrders.Dto;

namespace BIP.InternalCRM.WebApi.PurchaseOrders;

public class PurchaseOrderDtoProfile : Profile
{
    public PurchaseOrderDtoProfile()
    {
        ShouldUseConstructor = (t) => t.IsPublic;

        CreateMap<AddPurchaseOrderDto, AddPurchaseOrderCommand>()
            .ForRecordParameter(
                _ => _.ProductName,
                o => o.MapFrom(src => src.ProductName))
            .ForRecordParameter(
                _ => _.CustomerName,
                o => o.MapFrom(src => src.CustomerName));

        CreateMap<PurchaseOrder, PurchaseOrderDto>()
            .ForRecordParameter(_ => _.CustomerName, o => o.MapFrom<string?>(_ => null))
            .ForRecordParameter(_ => _.ProductName, o => o.MapFrom<string?>(_ => null));
        CreateMap<PurchaseOrderQueryResult, PurchaseOrderDto>()
            .IncludeMembers(_ => _.PurchaseOrder);
    }
}
