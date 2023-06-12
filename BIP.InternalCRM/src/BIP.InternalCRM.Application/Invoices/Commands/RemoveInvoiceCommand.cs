using BIP.InternalCRM.Application.AppErrors;
using BIP.InternalCRM.Application.Contexts;
using BIP.InternalCRM.Application.Extensions;
using BIP.InternalCRM.Domain.Invoices;
using MediatR;
using Microsoft.EntityFrameworkCore;
using OneOf;
using Success = OneOf.Types.Success;

namespace BIP.InternalCRM.Application.Invoices.Commands;

public record RemoveInvoiceCommand(
    string Number
) : IRequest<OneOf<Success, NotFound>>
{
    public class Handler :
        IRequestHandler<RemoveInvoiceCommand, OneOf<Success, NotFound>>
    {
        private readonly IDomainDbContext _dbContext;
        private readonly IUnitOfWork _unitOfWork;

        public Handler(IDomainDbContext dbContext, IUnitOfWork unitOfWork)
        {
            _dbContext = dbContext;
            _unitOfWork = unitOfWork;
        }

        public async Task<OneOf<Success, NotFound>> Handle(RemoveInvoiceCommand request, CancellationToken cancellationToken)
        {
            var invoice = await _dbContext.Invoices
                .Where(_ => _.Number == request.Number)
                .Include(_ => _.Subscriptions)
                .Include(_ => _.Payments)
                .FirstOrDefaultAsync(cancellationToken);

            if (invoice is null) return new NotFound<Invoice>();
            
            var result = invoice.Remove();
            
            _dbContext.Invoices.Remove(invoice);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return result;
        }
    }
}