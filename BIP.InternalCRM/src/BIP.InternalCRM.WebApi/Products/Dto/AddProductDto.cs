namespace BIP.InternalCRM.WebApi.Products.Dto;

public record AddProductDto(
    string Name,
    IFormFile? Icon,
    IFormFile? IlProject
);