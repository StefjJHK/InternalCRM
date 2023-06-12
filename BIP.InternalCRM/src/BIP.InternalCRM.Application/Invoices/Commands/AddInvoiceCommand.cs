using BIP.InternalCRM.Application.AppErrors;
using BIP.InternalCRM.Application.Contexts;
using BIP.InternalCRM.Application.Extensions;
using BIP.InternalCRM.Domain.Customers;
using BIP.InternalCRM.Domain.DomainErrors;
using BIP.InternalCRM.Domain.Invoices;
using BIP.InternalCRM.Domain.Products;
using BIP.InternalCRM.Domain.PurchaseOrders;
using MediatR;
using Microsoft.EntityFrameworkCore;
using OneOf;

namespace BIP.InternalCRM.Application.Invoices.Commands;

public record AddInvoiceCommand(
    string Number,
    decimal Amount,
    DateTime ReceivedDate,
    DateTime DueDate,
    string ProductName,
    string CustomerName,
    string? PurchaseOrderNumber = null
) : IRequest<OneOf<Invoice, NotFound, DomainError>>
{
    public class Handler
        : IRequestHandler<AddInvoiceCommand, OneOf<Invoice, NotFound, DomainError>>
    {
        private readonly IDomainDbContext _dbContext;
        private readonly IUnitOfWork _unitOfWork;

        public Handler(IDomainDbContext dbContext, IUnitOfWork unitOfWork)
        {
            _dbContext = dbContext;
            _unitOfWork = unitOfWork;
        }

        public async Task<OneOf<Invoice, NotFound, DomainError>> Handle(
            AddInvoiceCommand request,
            CancellationToken cancellationToken)
        {
            if (request.PurchaseOrderNumber.IsNotEmpty())
            {
                var existingPo = await _dbContext.PurchaseOrders
                    .UseIf(
                        request.PurchaseOrderNumber.IsNotEmpty(),
                        q => q.Where(_ => _.Number == request.PurchaseOrderNumber))
                    .FirstOrDefaultAsync(cancellationToken);

                if (existingPo is null) return new NotFound<PurchaseOrder>();

                var invoiceViaPo = existingPo.AddInvoice(
                    new InvoiceId(Guid.NewGuid()),
                    request.Number,
                    request.Amount,
                    request.ReceivedDate,
                    request.DueDate);

                if (invoiceViaPo.Value is DomainError domainErrorViaPo) return domainErrorViaPo;
                
                _dbContext.PurchaseOrders.Update(existingPo);
                await _unitOfWork.SaveChangesAsync(cancellationToken);

                return invoiceViaPo.AsT0;
            }

            var existingProduct = await _dbContext.Products
                .AsNoTracking()
                .UseIf(request.ProductName.IsNotEmpty(), q => q.Where(_ => _.Name == request.ProductName))
                .FirstOrDefaultAsync(cancellationToken);

            if (existingProduct is null) return new NotFound<Product>();

            var existingCustomer = await _dbContext.Customers
                .AsNoTracking()
                .UseIf(request.CustomerName.IsNotEmpty(), q => q.Where(_ => _.Name == request.CustomerName))
                .FirstOrDefaultAsync(cancellationToken);
            
            if (existingCustomer is null) return new NotFound<Customer>();

            var otherInvoicesNumbers = await _dbContext.Invoices
                .Select(_ => _.Number)
                .ToListAsync(cancellationToken);

            var invoice = Invoice.Create(
                new InvoiceId(Guid.NewGuid()),
                request.Number,
                otherInvoicesNumbers,
                request.Amount,
                request.ReceivedDate,
                request.DueDate,
                existingProduct.Id,
                existingCustomer.Id);

            if (invoice.Value is DomainError domainError) return domainError;

            await _dbContext.Invoices.AddAsync(invoice.AsT0, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return invoice.AsT0;
        }
    }
}