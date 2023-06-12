using AutoMapper;
using BIP.InternalCRM.Application.Extensions;
using BIP.InternalCRM.Application.Leads.Commands;
using BIP.InternalCRM.Application.Leads.Queries;
using BIP.InternalCRM.Application.Leads.QueryResults;
using BIP.InternalCRM.Domain.Leads;
using BIP.InternalCRM.WebApi.Leads.Dto;

namespace BIP.InternalCRM.WebApi.Leads;

public class LeadDtoProfile : Profile
{
    public LeadDtoProfile()
    {
        ShouldUseConstructor = t => t.IsPublic;
        
        CreateMap<AddLeadDto, AddLeadCommand>()
            .ForRecordParameter(
                _ => _.ProductName,
                o => o.MapFrom(src => src.ProductName))
            .ForRecordParameter(
                _ => _.Fullname,
                o => o.MapFrom(src => src.ContactName))
            .ForRecordParameter(
                _ => _.PhoneNumber,
                o => o.MapFrom(src => src.ContactPhone))
            .ForRecordParameter(
                _ => _.Email,
                o => o.MapFrom(src => src.ContactEmail));
        
        CreateMap<ChangeLeadDto, ChangeLeadCommand>()
            .ForRecordParameter(
                _ => _.NameIdentity,
                o => o.MapFromItems<ChangeLeadDto, string>(nameof(ChangeLeadCommand.NameIdentity)))
            .ForRecordParameter(
                _ => _.Fullname,
                o => o.MapFrom(src => src.ContactName))
            .ForRecordParameter(
                _ => _.PhoneNumber,
                o => o.MapFrom(src => src.ContactPhone))
            .ForRecordParameter(
                _ => _.Email,
                o => o.MapFrom(src => src.ContactEmail));

        CreateMap<Lead, LeadDto>()
            .ForRecordParameter(_ => _.ProductName, o => o.MapFrom<string?>(_ => null))
            .ForRecordParameter(
                _ => _.ContactName,
                o => o.MapFrom(src => src.ContactInfo.Fullname))
            .ForRecordParameter(
                _ => _.PhoneNumber,
                o => o.MapFrom(src => src.ContactInfo.PhoneNumber))
            .ForRecordParameter(
                _ => _.Email,
                o => o.MapFrom(src => src.ContactInfo.Email));
        CreateMap<LeadQueryResult, LeadDto>()
            .IncludeMembers(_ => _.Lead)
            .ForRecordParameter(
                _ => _.ContactName,
                o => o.MapFrom(src => src.Lead.ContactInfo.Fullname))
            .ForRecordParameter(
                _ => _.PhoneNumber,
                o => o.MapFrom(src => src.Lead.ContactInfo.PhoneNumber))
            .ForRecordParameter(
                _ => _.Email,
                o => o.MapFrom(src => src.Lead.ContactInfo.Email));
    }
}
