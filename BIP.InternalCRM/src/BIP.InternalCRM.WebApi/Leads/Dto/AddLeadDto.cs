namespace BIP.InternalCRM.WebApi.Leads.Dto;

public record AddLeadDto(
    string Name,
    string ContactName,
    string ContactPhone,
    string ContactEmail,
    string ProductName,
    decimal Cost,
    DateTime StartDate,
    DateTime EndDate
);