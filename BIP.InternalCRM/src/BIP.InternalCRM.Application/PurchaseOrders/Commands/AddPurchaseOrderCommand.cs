using BIP.InternalCRM.Application.AppErrors;
using BIP.InternalCRM.Application.Contexts;
using BIP.InternalCRM.Domain.Customers;
using BIP.InternalCRM.Domain.DomainErrors;
using BIP.InternalCRM.Domain.Products;
using BIP.InternalCRM.Domain.PurchaseOrders;
using MediatR;
using Microsoft.EntityFrameworkCore;
using OneOf;

namespace BIP.InternalCRM.Application.PurchaseOrders.Commands;

public record AddPurchaseOrderCommand(
    string Number,
    decimal Amount,
    string CustomerName,
    string ProductName,
    DateTime ReceivedDate,
    DateTime DueDate
) : IRequest<OneOf<PurchaseOrder, NotFound, DomainError>>
{
    public class Handler
        : IRequestHandler<AddPurchaseOrderCommand, OneOf<PurchaseOrder, NotFound, DomainError>>
    {
        private readonly IDomainDbContext _dbContext;
        private readonly IUnitOfWork _unitOfWork;

        public Handler(IDomainDbContext dbContext, IUnitOfWork unitOfWork)
        {
            _dbContext = dbContext;
            _unitOfWork = unitOfWork;
        }

        public async Task<OneOf<PurchaseOrder, NotFound, DomainError>> Handle(
            AddPurchaseOrderCommand request,
            CancellationToken cancellationToken)
        {
            var existingProduct = await _dbContext.Products
                .Where(_ => _.Name == request.ProductName)
                .FirstOrDefaultAsync(cancellationToken);

            if (existingProduct is null) return new NotFound<Product>();

            var existingCustomer = await _dbContext.Customers
                .Where(_ => _.Name == request.CustomerName)
                .FirstOrDefaultAsync(cancellationToken);
            
            if (existingCustomer is null) return new NotFound<Customer>();
            
            var otherPosNumbers = await _dbContext.PurchaseOrders
                .Select(_ => _.Number)
                .ToListAsync(cancellationToken);

            var result = PurchaseOrder.Create(
                new PurchaseOrderId(Guid.NewGuid()),
                request.Number,
                otherPosNumbers,
                request.Amount,
                existingProduct.Id,
                existingCustomer.Id,
                request.ReceivedDate,
                request.DueDate);

            if (result.Value is DomainError domainError) return domainError;
            
            await _dbContext.PurchaseOrders.AddAsync(result.AsT0, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            
            return result.AsT0;
        }
    }
}