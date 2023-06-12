using BIP.InternalCRM.Application.AppErrors;
using BIP.InternalCRM.Application.Contexts;
using BIP.InternalCRM.Application.Extensions;
using BIP.InternalCRM.Domain.DomainErrors;
using BIP.InternalCRM.Domain.Invoices;
using MediatR;
using Microsoft.EntityFrameworkCore;
using OneOf;

namespace BIP.InternalCRM.Application.Invoices.Commands;

public record ChangeInvoiceCommand(
    string NumberIdentity,
    string Number,
    decimal Amount,
    DateTime ReceivedDate,
    DateTime DueDate
) : IRequest<OneOf<Invoice, NotFound, DomainError>>
{
    public class Handler :
        IRequestHandler<ChangeInvoiceCommand, OneOf<Invoice, NotFound, DomainError>>
    {
        private readonly IDomainDbContext _dbContext;
        private readonly IUnitOfWork _unitOfWork;

        public Handler(IDomainDbContext dbContext, IUnitOfWork unitOfWork)
        {
            _dbContext = dbContext;
            _unitOfWork = unitOfWork;
        }

        public async Task<OneOf<Invoice, NotFound, DomainError>> Handle(
            ChangeInvoiceCommand request,
            CancellationToken cancellationToken)
        {
            var existing = await _dbContext.Invoices
                .UseIf(request.NumberIdentity.IsNotEmpty(), q => q.Where(_ => _.Number == request.NumberIdentity))
                .FirstOrDefaultAsync(cancellationToken);

            if (existing is null) return new NotFound<Invoice>();

            var otherInvoicesNumbers = await _dbContext.Invoices
                .Where(_ => _.Id != existing.Id)
                .Select(_ => _.Number)
                .ToListAsync(cancellationToken);

            IOneOf changeResult = (OneOf<Invoice>)existing;

            if (existing.Number.NotEquals(request.Number))
            {
                changeResult = existing.ChangeNumber(request.Number, otherInvoicesNumbers);
                if (changeResult.Value is DomainError domainError) return domainError;
            }

            if (existing.Amount.NotEquals(request.Amount))
            {
                changeResult = existing.ChangeAmount(request.Amount);
                if (changeResult.Value is DomainError domainError) return domainError;
            }
            
            if (existing.ReceivedDate.NotEquals(request.ReceivedDate)
                || existing.DueDate.NotEquals(request.DueDate))
            {
                changeResult = (OneOf<Invoice>)existing.ChangeExpirationDateRange(request.ReceivedDate, request.DueDate);
            }
            
            _dbContext.Invoices.Update((changeResult.Value as Invoice)!);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return (changeResult.Value as Invoice)!;
        }
    }
}