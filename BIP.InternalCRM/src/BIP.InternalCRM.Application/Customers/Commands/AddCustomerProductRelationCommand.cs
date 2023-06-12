using BIP.InternalCRM.Application.Contexts;
using BIP.InternalCRM.Application.Customers.IntegrationEvents;
using BIP.InternalCRM.Domain.Customers;
using BIP.InternalCRM.Domain.Products;
using MediatR;

namespace BIP.InternalCRM.Application.Customers.Commands;

public record AddCustomerProductRelationCommand(
    CustomerId CustomerId,
    ProductId ProductId
) : IRequest
{
    public class Handler :
        IRequestHandler<AddCustomerProductRelationCommand>
    {
        private readonly IRelationsDbContext _relationsDbContext;
        private readonly IPublisher _publisher;

        public Handler(
            IRelationsDbContext relationsDbContext,
            IPublisher publisher)
        {
            _relationsDbContext = relationsDbContext;
            _publisher = publisher;
        }

        public async Task Handle(AddCustomerProductRelationCommand request,
            CancellationToken cancellationToken)
        {
            await _relationsDbContext.CustomersRelations.AddRangeAsync(
                new CustomerRelations(request.CustomerId, request.ProductId));
            
            await _publisher.Publish(
                new CustomerProductRelationCreatedIntegrationEvent(Guid.NewGuid(), request.CustomerId, request.ProductId),
                cancellationToken);
        }
    }
}