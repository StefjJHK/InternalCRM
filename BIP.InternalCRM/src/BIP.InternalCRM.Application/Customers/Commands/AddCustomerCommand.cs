using BIP.InternalCRM.Domain.Customers;
using BIP.InternalCRM.Domain.DomainErrors;
using MediatR;
using OneOf;

namespace BIP.InternalCRM.Application.Customers.Commands;

public record AddCustomerCommand(
    string Name,
    string Fullname,
    string PhoneNumber,
    string Email
) : IRequest<OneOf<Customer, DomainError>>;
