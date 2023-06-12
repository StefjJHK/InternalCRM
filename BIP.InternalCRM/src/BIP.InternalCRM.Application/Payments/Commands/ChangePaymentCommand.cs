using BIP.InternalCRM.Application.AppErrors;
using BIP.InternalCRM.Application.Contexts;
using BIP.InternalCRM.Application.Extensions;
using BIP.InternalCRM.Domain.DomainErrors;
using BIP.InternalCRM.Domain.Invoices;
using BIP.InternalCRM.Domain.Payments;
using MediatR;
using Microsoft.EntityFrameworkCore;
using OneOf;

namespace BIP.InternalCRM.Application.Payments.Commands;

public record ChangePaymentCommand(
    string InvoiceNumber,
    string PaymentNumberIdentity,
    string Number,
    decimal Amount,
    DateTime ReceivedDate
) : IRequest<OneOf<Payment, NotFound, DomainError>>
{
    public class Handler :
        IRequestHandler<ChangePaymentCommand, OneOf<Payment, NotFound, DomainError>>
    {
        private readonly IDomainDbContext _dbContext;
        private readonly IUnitOfWork _unitOfWork;

        public Handler(IDomainDbContext dbContext, IUnitOfWork unitOfWork)
        {
            _dbContext = dbContext;
            _unitOfWork = unitOfWork;
        }

        public async Task<OneOf<Payment, NotFound, DomainError>> Handle(ChangePaymentCommand request, CancellationToken cancellationToken)
        {
            var existingInvoice = await _dbContext.Invoices
                .Include(_ => _.Payments)
                .Include(_ => _.Subscriptions)
                .Where(_ => _.Number == request.InvoiceNumber)
                .FirstOrDefaultAsync(cancellationToken);

            if (existingInvoice is null) return new NotFound<Invoice>();

            var existingPayment = existingInvoice.Payments
                .FirstOrDefault(_ => _.Number == request.PaymentNumberIdentity);
            if (existingPayment is null) return new NotFound<Payment>();

            var changedPayment = existingPayment;
            
            if (existingPayment.Number.NotEquals(request.Number))
            {
                var changeNumberResult = existingInvoice.ChangePaymentNumber(existingPayment.Id, request.Number);
                if (changeNumberResult.Value is DomainError domainError) return domainError;
                changedPayment = changeNumberResult.AsT0;
            }

            if (existingPayment.Amount.NotEquals(request.Amount))
            {
                changedPayment = existingInvoice.ChangePaymentAmount(existingPayment.Id, request.Amount);
            }
            
            if (existingPayment.ReceivedDate.NotEquals(request.ReceivedDate))
            {
                var changeNumberResult = existingInvoice
                    .ChangePaymentReceivedDate(existingPayment.Id, request.ReceivedDate);
                if (changeNumberResult.Value is DomainError domainError) return domainError;
                changedPayment = changeNumberResult.AsT0;
            }
            
            _dbContext.Payments.Update(changedPayment);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return changedPayment;
        }
    }
}