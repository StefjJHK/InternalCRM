using BIP.InternalCRM.Application.AppErrors;
using BIP.InternalCRM.Application.Contexts;
using BIP.InternalCRM.Domain.DomainErrors;
using BIP.InternalCRM.Domain.Invoices;
using BIP.InternalCRM.Domain.Subscriptions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using OneOf;

namespace BIP.InternalCRM.Application.Subscriptions.Commands;

public record ChangeSubscriptionCommand(
    string InvoiceNumber,
    string SubNumberIdentity,
    string SubLegalEntity,
    decimal Cost,
    DateTime ValidFrom,
    DateTime ValidUntil
) : IRequest<OneOf<Subscription, NotFound, DomainError>>
{
    public class Handler :
        IRequestHandler<ChangeSubscriptionCommand, OneOf<Subscription, NotFound, DomainError>>
    {
        private readonly IDomainDbContext _dbContext;
        private readonly IUnitOfWork _unitOfWork;

        public Handler(IDomainDbContext dbContext, IUnitOfWork unitOfWork)
        {
            _dbContext = dbContext;
            _unitOfWork = unitOfWork;
        }

        public async Task<OneOf<Subscription, NotFound, DomainError>> Handle(ChangeSubscriptionCommand request,
            CancellationToken cancellationToken)
        {
            var invoice = await _dbContext.Invoices
                .Include(_ => _.Subscriptions)
                .Where(_ => _.Number == request.InvoiceNumber)
                .FirstOrDefaultAsync(cancellationToken);

            if (invoice is null) return new NotFound<Invoice>();

            var sub = invoice.Subscriptions
                .FirstOrDefault(_ => _.Number == request.SubNumberIdentity);
            if (sub is null) return new NotFound<Subscription>();

            // var changeResult = existingResult.ChainOfWithConditions(
            //     (_ => _.Cost.NotEquals(request.Cost),
            //         _ => _.ChangeCost(request.Cost)),
            //     (_ => _.SubLegalEntity.NotEquals(request.SubLegalEntity),
            //         _ => _.ChangeSubLegalEntity(request.SubLegalEntity)),
            //     (_ => _.ValidFrom.NotEquals(request.ValidFrom)
            //           || _.ValidUntil.NotEquals(request.ValidUntil),
            //         _ => _.ChangeValidDateRange(request.ValidFrom, request.ValidUntil)));
            //
            // if (changeResult.IsFailure)
            // {
            //     return changeResult;
            // }
            //
            // _dbContext.Subscriptions.Update(changeResult.Value);
            // await _unitOfWork.SaveChangesAsync(cancellationToken);

            return sub;
        }
    }
}