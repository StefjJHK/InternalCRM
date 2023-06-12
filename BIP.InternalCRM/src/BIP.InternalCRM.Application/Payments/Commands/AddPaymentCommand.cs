using BIP.InternalCRM.Application.AppErrors;
using BIP.InternalCRM.Application.Contexts;
using BIP.InternalCRM.Domain.DomainErrors;
using BIP.InternalCRM.Domain.Invoices;
using BIP.InternalCRM.Domain.Payments;
using MediatR;
using Microsoft.EntityFrameworkCore;
using OneOf;

namespace BIP.InternalCRM.Application.Payments.Commands;

public record AddPaymentCommand(
    string InvoiceNumber,
    string Number,
    decimal Amount,
    DateTime ReceivedDate
) : IRequest<OneOf<Payment, NotFound, DomainError>>
{
    public class Handler
        : IRequestHandler<AddPaymentCommand, OneOf<Payment, NotFound, DomainError>>
    {
        private readonly IDomainDbContext _dbContext;
        private readonly IUnitOfWork _unitOfWork;

        public Handler(IDomainDbContext dbContext, IUnitOfWork unitOfWork)
        {
            _dbContext = dbContext;
            _unitOfWork = unitOfWork;
        }

        public async Task<OneOf<Payment, NotFound, DomainError>> Handle(
            AddPaymentCommand request,
            CancellationToken cancellationToken)
        {
            var invoice = await _dbContext.Invoices
                .Include(_ => _.Payments)
                .Include(_ => _.Subscriptions)
                .Include(_ => _.PurchaseOrder)
                .Where(_ => _.Number == request.InvoiceNumber)
                .FirstOrDefaultAsync(cancellationToken);

            if (invoice is null) return new NotFound<Invoice>();

            var result = invoice.AddPayment(
                new PaymentId(Guid.NewGuid()),
                request.Number,
                request.Amount,
                request.ReceivedDate);

            if (result.Value is DomainError domainError) return domainError;

            await _dbContext.Payments.AddAsync(result.AsT0, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return result.AsT0;
        }
    }
}