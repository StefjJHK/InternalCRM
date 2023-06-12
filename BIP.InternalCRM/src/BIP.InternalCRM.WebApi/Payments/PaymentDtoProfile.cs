using AutoMapper;
using BIP.InternalCRM.Application.Extensions;
using BIP.InternalCRM.Application.Payments.Commands;
using BIP.InternalCRM.Application.Payments.QueryResult;
using BIP.InternalCRM.Domain.Payments;
using BIP.InternalCRM.WebApi.Invoices.Dto;
using BIP.InternalCRM.WebApi.Payments.Dtos;

namespace BIP.InternalCRM.WebApi.Payments;

public class PaymentDtoProfile : Profile
{
    public PaymentDtoProfile()
    {
        ShouldUseConstructor = t => t.IsPublic;

        CreateMap<AddPaymentDto, AddPaymentCommand>()
            .ForRecordParameter(
                _ => _.InvoiceNumber,
                o => o.MapFromItems<AddPaymentDto, string>(nameof(AddPaymentCommand.InvoiceNumber)));

        CreateMap<ChangePaymentDto, ChangePaymentCommand>()
            .ForRecordParameter(
                _ => _.InvoiceNumber,
                o => o.MapFromItems<ChangePaymentDto, string>(nameof(ChangePaymentCommand.InvoiceNumber)))
            .ForRecordParameter(
                _ => _.PaymentNumberIdentity,
                o => o.MapFromItems<ChangePaymentDto, string>(nameof(ChangePaymentCommand.PaymentNumberIdentity)));

        CreateMap<Payment, PaymentDto>()
            .ForRecordParameter(_ => _.InvoiceNumber, o => o.MapFrom<string?>(_ => null));
        CreateMap<PaymentQueryResult, PaymentDto>()
            .IncludeMembers(_ => _.Payment);
    }
}