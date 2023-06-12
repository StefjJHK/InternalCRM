using BIP.InternalCRM.Application.Primitives;
using BIP.InternalCRM.Domain.Customers;
using BIP.InternalCRM.Domain.Products;

namespace BIP.InternalCRM.Application.Customers.IntegrationEvents;

public record CustomerProductRelationCreatedIntegrationEvent(
    Guid Id,
    CustomerId CustomerId,
    ProductId ProductId
) : IntegrationEvent(Id);