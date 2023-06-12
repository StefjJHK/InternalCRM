namespace BIP.InternalCRM.WebApi.Leads.Dto;

public record LeadDto(
    string Name,
    string ContactName,
    string PhoneNumber,
    string Email,
    string ProductName,
    decimal Cost,
    DateTime StartDate,
    DateTime EndDate
);
