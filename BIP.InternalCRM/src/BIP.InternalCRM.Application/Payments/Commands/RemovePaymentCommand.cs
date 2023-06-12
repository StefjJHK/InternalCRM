using BIP.InternalCRM.Application.AppErrors;
using BIP.InternalCRM.Application.Contexts;
using BIP.InternalCRM.Domain.Invoices;
using BIP.InternalCRM.Domain.Payments;
using MediatR;
using Microsoft.EntityFrameworkCore;
using OneOf;
using OneOf.Types;
using NotFound = BIP.InternalCRM.Application.AppErrors.NotFound;

namespace BIP.InternalCRM.Application.Payments.Commands;

public record RemovePaymentCommand(
    string InvoiceNumber,
    string PaymentNumber
) : IRequest<OneOf<Success, NotFound>>
{
    public class Handler :
        IRequestHandler<RemovePaymentCommand, OneOf<Success, NotFound>>
    {
        private readonly IDomainDbContext _dbContext;
        private readonly IUnitOfWork _unitOfWork;

        public Handler(IDomainDbContext dbContext, IUnitOfWork unitOfWork)
        {
            _dbContext = dbContext;
            _unitOfWork = unitOfWork;
        } 
        
        public async Task<OneOf<Success, NotFound>> Handle(RemovePaymentCommand request, CancellationToken cancellationToken)
        {
            var existingInvoice = await _dbContext.Invoices
                .Include(_ => _.Payments)
                .Include(_ => _.Subscriptions)
                .Where(_ => _.Number == request.InvoiceNumber)
                .FirstOrDefaultAsync(cancellationToken);

            if (existingInvoice is null) return new NotFound<Invoice>();

            var existingPayment = existingInvoice.Payments
                .FirstOrDefault(_ => _.Number == request.PaymentNumber);
            if (existingPayment is null) return new NotFound<Payment>();

            var removeResult = existingInvoice.RemovePayment(existingPayment.Id);

            _dbContext.Payments.Remove(existingPayment);
            _dbContext.Invoices.Update(existingInvoice);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return removeResult;
        }
    }
}