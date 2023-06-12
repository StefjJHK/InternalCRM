namespace BIP.InternalCRM.WebApi.Products.Dto;

public record ChangeProductDto(
    string Name,
    IFormFile? Icon,
    IFormFile? IlProject
);