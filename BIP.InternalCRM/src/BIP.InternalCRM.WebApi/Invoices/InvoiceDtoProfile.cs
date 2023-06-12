using AutoMapper;
using BIP.InternalCRM.Application.Extensions;
using BIP.InternalCRM.Application.Invoices.Commands;
using BIP.InternalCRM.Application.Invoices.QueryResults;
using BIP.InternalCRM.Domain.Invoices;
using BIP.InternalCRM.WebApi.Invoices.Dto;

namespace BIP.InternalCRM.WebApi.Invoices;

public class InvoiceDtoProfile : Profile
{
    public InvoiceDtoProfile()
    {
        ShouldUseConstructor = t => t.IsPublic;

        CreateMap<AddInvoiceDto, AddInvoiceCommand>()
            .ForRecordParameter(
                _ => _.PurchaseOrderNumber,
                o => o.MapFrom(src => src.PurchaseOrderNumber))
            .ForRecordParameter(
                _ => _.ProductName,
                o => o.MapFrom(src => src.ProductName))
            .ForRecordParameter(
                _ => _.CustomerName,
                o => o.MapFrom(src => src.CustomerName));
        
        CreateMap<ChangeInvoiceDto, ChangeInvoiceCommand>()
            .ForRecordParameter(
                _ => _.NumberIdentity,
                o => o.MapFromItems<ChangeInvoiceDto, string>(nameof(ChangeInvoiceCommand.NumberIdentity)));

        CreateMap<Invoice, InvoiceDto>()
            .ForRecordParameter(_ => _.CustomerName, o => o.MapFrom<string?>(_ => null))
            .ForRecordParameter(_ => _.ProductName, o => o.MapFrom<string?>(_ => null));
        CreateMap<InvoiceQueryResult, InvoiceDto>()
            .IncludeMembers(_ => _.Invoice);

    }
}