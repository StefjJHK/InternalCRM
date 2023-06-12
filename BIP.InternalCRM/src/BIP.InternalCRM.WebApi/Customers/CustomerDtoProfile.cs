using AutoMapper;
using BIP.InternalCRM.Application.Customers.Commands;
using BIP.InternalCRM.Application.Extensions;
using BIP.InternalCRM.Domain.Customers;
using BIP.InternalCRM.WebApi.Customers.Dto;

namespace BIP.InternalCRM.WebApi.Customers;

public class CustomerDtoProfile : Profile
{
    public CustomerDtoProfile()
    {
        ShouldUseConstructor = t => t.IsPublic;

        CreateMap<AddCustomerDto, AddCustomerCommand>()
            .ForRecordParameter(
                _ => _.Fullname,
                o => o.MapFrom(src => src.ContactName))
            .ForRecordParameter(
                _ => _.PhoneNumber,
                o => o.MapFrom(src => src.ContactPhone))
            .ForRecordParameter(
                _ => _.Email,
                o => o.MapFrom(src => src.ContactEmail));

        CreateMap<Customer, CustomerDto>()
            .ForRecordParameter(
                _ => _.ContactName,
                o => o.MapFrom(src => src.ContactInfo.Fullname))
            .ForRecordParameter(
                _ => _.PhoneNumber,
                o => o.MapFrom(src => src.ContactInfo.PhoneNumber))
            .ForRecordParameter(
                _ => _.Email,
                o => o.MapFrom(src => src.ContactInfo.Email));

        CreateMap<ChangeCustomerDto, ChangeCustomerCommand>()
            .ForRecordParameter(
                _ => _.NameIdentity,
                o => o.MapFromItems<ChangeCustomerDto, string>(
                    nameof(ChangeCustomerCommand.NameIdentity)))
            .ForRecordParameter(
                _ => _.Fullname,
                o => o.MapFrom(src => src.ContactName))
            .ForRecordParameter(
                _ => _.PhoneNumber,
                o => o.MapFrom(src => src.ContactPhone))
            .ForRecordParameter(
                _ => _.Email,
                o => o.MapFrom(src => src.ContactEmail));
    }
}