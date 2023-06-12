using BIP.InternalCRM.Application.AppErrors;
using BIP.InternalCRM.Domain.Customers;
using BIP.InternalCRM.Domain.DomainErrors;
using BIP.InternalCRM.Domain.Leads;
using MediatR;
using OneOf;

namespace BIP.InternalCRM.Application.Customers.Commands;

public record AddCustomerFromLeadCommand(
    string LeadName
) : IRequest<OneOf<Customer, NotFound<Lead>, DomainError>>;