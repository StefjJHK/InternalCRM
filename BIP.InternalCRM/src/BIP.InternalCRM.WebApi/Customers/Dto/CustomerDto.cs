namespace BIP.InternalCRM.WebApi.Customers.Dto;

public record CustomerDto(
    string Name,
    string ContactName,
    string PhoneNumber,
    string Email
);
