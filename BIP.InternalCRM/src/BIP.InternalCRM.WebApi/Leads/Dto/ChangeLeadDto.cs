namespace BIP.InternalCRM.WebApi.Leads.Dto;

public record ChangeLeadDto(
    string Name,
    string ContactName,
    string ContactPhone,
    string ContactEmail,
    decimal Cost,
    DateTime StartDate,
    DateTime EndDate
);