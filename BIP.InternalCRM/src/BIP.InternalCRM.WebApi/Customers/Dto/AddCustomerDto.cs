namespace BIP.InternalCRM.WebApi.Customers.Dto;

public record AddCustomerDto(
    string Name,
    string ContactName,
    string ContactPhone,
    string ContactEmail
);