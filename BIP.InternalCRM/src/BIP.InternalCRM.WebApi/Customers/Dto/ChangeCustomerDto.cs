namespace BIP.InternalCRM.WebApi.Customers.Dto;

public record ChangeCustomerDto(
    string Name,
    string ContactName,
    string ContactPhone,
    string ContactEmail
);