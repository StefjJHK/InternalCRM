using BIP.InternalCRM.Application.AppErrors;
using BIP.InternalCRM.Application.Contexts;
using BIP.InternalCRM.Application.Payments.QueryResult;
using BIP.InternalCRM.Domain.Payments;
using MediatR;
using Microsoft.EntityFrameworkCore;
using OneOf;

namespace BIP.InternalCRM.Application.Payments.Queries;

public record GetPaymentQuery(
    string InvoiceNumber,
    string PaymentNumber
) : IRequest<OneOf<PaymentQueryResult, NotFound<Payment>>>
{
    public class Handler :
        IRequestHandler<GetPaymentQuery, OneOf<PaymentQueryResult, NotFound<Payment>>>
    {
        private readonly IDomainDbContext _dbContext;

        public Handler(IDomainDbContext dbContext)
        {
            _dbContext = dbContext; 
        }

        public async Task<OneOf<PaymentQueryResult, NotFound<Payment>>> Handle(
            GetPaymentQuery request,
            CancellationToken cancellationToken)
        {
            var result = await _dbContext.Invoices
                .Where(_ => _.Number.Equals(request.InvoiceNumber))
                .SelectMany(_ => _.Payments)
                .Where(_ => _.Number.Equals(request.PaymentNumber))
                .FirstOrDefaultAsync(cancellationToken);

            return result is not null
                ? new PaymentQueryResult(result, request.InvoiceNumber)
                : new NotFound<Payment>();
        }
    }
}