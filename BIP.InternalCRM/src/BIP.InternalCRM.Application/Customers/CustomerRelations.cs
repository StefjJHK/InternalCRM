using BIP.InternalCRM.Domain.Customers;
using BIP.InternalCRM.Domain.Products;

namespace BIP.InternalCRM.Application.Customers;

public record CustomerRelations(
    CustomerId CustomerId,
    ProductId ProductId
);