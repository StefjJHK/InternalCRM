using AutoMapper;
using BIP.InternalCRM.Application.Extensions;
using BIP.InternalCRM.Application.Products.Commands;
using BIP.InternalCRM.Application.Statistics.Products.Models;
using BIP.InternalCRM.Domain.Products;
using BIP.InternalCRM.WebApi.Extensions;
using BIP.InternalCRM.WebApi.Products.Dto;

namespace BIP.InternalCRM.WebApi.Products;

public class ProductDtoProfile : Profile
{
    public ProductDtoProfile()
    {
        ShouldUseConstructor = _ => _.IsPublic;

        CreateMap<AddProductDto, AddProductCommand>()
            .ForRecordParameter(
                _ => _.MediaType,
                o => o.MapFrom(src => src.Icon != null ? src.Icon.ContentType : null))
            .ForRecordParameter(
                _ => _.IconData,
                o => o.MapFrom(src => src.Icon != null ? src.Icon.ReadBytes() : null))
            .ForRecordParameter(
                _ => _.OriginalFilename,
                o => o.MapFrom(src => src.IlProject != null
                    ? Path.GetFileNameWithoutExtension(src.IlProject.FileName)
                    : null))
            .ForRecordParameter(
                _ => _.IlProjectData,
                o => o.MapFrom(src => src.IlProject != null ? src.IlProject.ReadBytes() : null));
        
        CreateMap<ChangeProductDto, ChangeProductCommand>()
            .ForRecordParameter(
                _ => _.NameIdentity,
                o => o.MapFromItems<ChangeProductDto, string>(nameof(ChangeProductCommand.NameIdentity)))
            .ForRecordParameter(
                _ => _.MediaType,
                o => o.MapFrom(src => src.Icon != null ? src.Icon.ContentType : null))
            .ForRecordParameter(
                _ => _.IconData,
                o => o.MapFrom(src => src.Icon != null ? src.Icon.ReadBytes() : null))
            .ForRecordParameter(
                _ => _.OriginalFilename,
                o => o.MapFrom(src => src.IlProject != null
                    ? Path.GetFileNameWithoutExtension(src.IlProject.FileName)
                    : null))
            .ForRecordParameter(
                _ => _.IlProjectData,
                o => o.MapFrom(src => src.IlProject != null ? src.IlProject.ReadBytes() : null));

        CreateMap<Product, ProductDto>()
            .ForRecordParameter(
                _ => _.IconUri,
                o => o.MapFrom(src =>
                    src.Icon != null
                        ? $"documents/images/{src.Icon.Filename}"
                        : null));

        CreateMap<ProductCustomersStatistics, ProductCustomersTableRowDto>();
    }
}