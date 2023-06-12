using AutoMapper;
using BIP.InternalCRM.Application.Statistics.Products.Models;

namespace BIP.InternalCRM.WebApi.Statistics.Products;

public class ProductStatisticsDtoProfile : Profile
{
    public ProductStatisticsDtoProfile()
    {
        ShouldUseConstructor = _ => _.IsPublic;

        CreateMap<ProductStatistics, ProductStatisticsDto>();
    }
}